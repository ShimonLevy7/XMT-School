import '../styles/TakeTest.css';

import { GiCheckMark, GiCrossMark } from "react-icons/gi";

import { useEffect, useState } from "react";
import { toast, ToastContainer } from 'react-toastify';

import { APIRequestTypes, IAnswer, IGetDisclosedTestResult, IMark, IQuestion, ISubmitTestResult, ITest, ITestWithStudentInfo, SelectedAnswer, SubmittableTest } from "../api/Types";
import { CurrentUserToken } from '../ts/Users';
import Loading from './Loading';
import { PostAsync } from '../api/API';
import { DEFAULT_TOAST_OPTIONS, MAX_TEST_POINTS } from '../App';
import { getTestWithSensitiveDataAsync } from '../api/Get';

export default function TakeTest({ testWithStudentInfo }: { testWithStudentInfo: ITestWithStudentInfo }) {
	const [isLoading, setIsLoading]: [boolean, Function] = useState(true);
	const [submitTestButtonEnabled, setSubmitTestButtonEnabled]: [boolean, Function] = useState(false);
	const [disableUi, setDisableUi]: [boolean, Function] = useState(false);
	const [mark, setMark]: [IMark | false, Function] = useState(false); // If has mark, took the test.
	const [testData, setTestData]: [ITest, Function] = useState(testWithStudentInfo.test);

	useEffect(() => {
		const fetchMark = async () => {
			if (testWithStudentInfo.wasTaken || !testWithStudentInfo.canTake) {
				setMark(testWithStudentInfo.mark);
				setSubmitTestButtonEnabled(false);

				// If you took the test, we can know the answers and show what's correct.
				const disclosedTest: string | IGetDisclosedTestResult = await getTestWithSensitiveDataAsync(CurrentUserToken, testWithStudentInfo.test.id);

				if (typeof (disclosedTest) !== 'string')
					setTestData(disclosedTest.data.test); // Use the full data and not just public data.
			}

			if (!testWithStudentInfo.canTake)
				setDisableUi(true);

			setIsLoading(false);
		};

		fetchMark();
	});

	const [selectedAnswers, setSelectedAnswers]: [SelectedAnswer[], Function] = useState([]);
	const markChecked = function (questionId: number, answerId: number) {
		const oldSelectedAnswer: number = selectedAnswers.findIndex(sa => sa.questionId === questionId);

		// Remove old selection.
		if (oldSelectedAnswer > -1)
			selectedAnswers.splice(oldSelectedAnswer, 1)

		const selectedAnswer: SelectedAnswer = new SelectedAnswer(questionId, answerId);

		selectedAnswers.push(selectedAnswer);

		setSelectedAnswers([...selectedAnswers]);

		setSubmitTestButtonEnabled(allChecked());
	}

	const allChecked = function () {
		if (selectedAnswers.length !== testData.questions.length)
			return false; // Not same length.

		return true;
	}

	const submitTest = async function () {
		if (!allChecked())
			return;

		const isUserSure: boolean = window.confirm('Are you sure that you\'d like to submit the test?\nThis is irreversible!');

		if (!isUserSure)
			return;

		// No more submitting.
		setSubmitTestButtonEnabled(false);
		setDisableUi(true);

		const result: string | ISubmitTestResult = await PostAsync<ISubmitTestResult>(APIRequestTypes.submitTest, {
			SubmittableTest: new SubmittableTest(testData.id, selectedAnswers)
		});

		if (typeof (result) === 'string')
			return alert(result);

		toast.info(`Your mark is: ${Math.round(result.data.mark.points)}/${MAX_TEST_POINTS}`, DEFAULT_TOAST_OPTIONS);

		setTimeout(() => {
			window.location.reload(); // Refresh the page.
		}, 5000);

		// Remove the "search" params.
		// window.location.href = window.location.pathname; // Actually no, I'd want them to see the results.
	}

	return (isLoading ? <Loading /> : <>
		<div className="take_test_container">
			<h1 className='test-title'>{testData.name}</h1>

			{mark ? <h2 className='test-title'>{`Mark: ${Math.round((mark as IMark).points)}/${MAX_TEST_POINTS}`}</h2> : <></>}

			<br />

			{testData.questions.map((question: IQuestion, _: number) => {
				return (<div key={`question-${question.id}`}>
					<fieldset className="take_test_question">
						<legend className="title centre">{question.questionText}</legend>

						{question.answers.map((answer: IAnswer, _: number) => {
							let checked: boolean | undefined = undefined;
							let correctAnswer: boolean = false;
							let wrongAnswer: boolean = false;

							if (mark) {
								const selectedAnswerFromMarkForThisQuestion: SelectedAnswer | undefined = (mark as IMark).selectedAnswers.find(sa => sa.questionId === question.id);

								// Is this answer checked?
								if (selectedAnswerFromMarkForThisQuestion) {
									checked = selectedAnswerFromMarkForThisQuestion.answerId === answer.id;
								}

								// Is this the correct answer?
								if (answer.isValidAnswer && checked)
									correctAnswer = true;

								// Did the student select a wrong answer?
								if (!correctAnswer && checked)
									wrongAnswer = true;
							}

							return (<div key={`answer-${answer.id}`} className='radio_wrapper'>
								<input checked={checked} readOnly={mark} disabled={mark || disableUi} onChange={() => markChecked(question.id, answer.id)} className='radio_input' type="radio" id={`answer-${answer.id}`} name={`question-${question.id}`} value="HTML" />
								<label className='radio_label' htmlFor={`answer-${answer.id}`}>{answer.answerText}</label>

								<div className='mark-icon'>
									{correctAnswer ? GiCheckMark({ size: 12, color: 'lightgreen' }) : <></>}
									{wrongAnswer ? GiCrossMark({ size: 12, color: 'red' }) : <></>}
								</div>
							</div>);
						})}
					</fieldset>

					<br />
				</div>);
			})}

			{mark ? <></> : <button disabled={!submitTestButtonEnabled} onClick={submitTest} className='btn btn-primary'>Submit Test</button>}

			<ToastContainer />
		</div>
	</>);
}
