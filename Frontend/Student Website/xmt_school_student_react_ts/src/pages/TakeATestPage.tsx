import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import { CurrentUserData, CurrentUserToken, UserTypes } from "../ts/Users";
import TakeTest from "../components/TakeTest";
import Error404Page from "./Error404";
import { APIRequestTypes, GetTestWithAuthorNameAndStudentInfoResult, IgetUndisclosedTestsWithStudentInfoResult, ITestWithStudentInfo } from "../api/Types";
import Loading from "../components/Loading";
import { getTestsWithAuthorNameAndStudentInfoAsync } from "../api/Get";
import ListTests from "./ListTests";
import { GetAsync } from "../api/API";
import Error400Page from "./Error400";

export default function TakeATestPage() {
	// Used for the requested test ID, if someone chose a test to view/take.
	const location = useLocation();

	const params = new URLSearchParams(location.search);
	const requestedTestId: string = params.get('testId') ?? '';
	const requestedStudentImposterId: string = params.get('imposterId') ?? '';

	// API.
	const [testsWithStudentInfo, setTestsWithStudentInfo]: [ITestWithStudentInfo[], Function] = useState([]);
	const [ready, setReady]: [boolean, Function] = useState(false);
	const [selectedTest, setSelectedTest]: [ITestWithStudentInfo | undefined, Function] = useState(undefined);

	useEffect(() => {
		const fetchTests = async () => {
			const testsResult: string | IgetUndisclosedTestsWithStudentInfoResult = await getTestsWithAuthorNameAndStudentInfoAsync(CurrentUserToken);

			if (typeof (testsResult) === 'string')
				return;

			setTestsWithStudentInfo(testsResult.data);
			setReady(true);
		};

		const fetchImposterData = async () => {
			if (!requestedStudentImposterId)
				return;

			if (CurrentUserData.type < UserTypes.Teacher)
				return;

			const result: string | GetTestWithAuthorNameAndStudentInfoResult = await GetAsync<GetTestWithAuthorNameAndStudentInfoResult>(APIRequestTypes.getTestWithAuthorNameAndStudentInfo, {
				Accept: 'application/json',
				TestId: requestedTestId,
				StudentId: requestedStudentImposterId
			});

			if (typeof (result) === 'string')
				return;

			if (!result.data.wasTaken)
				return;

			setSelectedTest(result.data);
		}

		fetchTests();
		fetchImposterData();
	}, [requestedStudentImposterId, requestedTestId]);

	const testId: number = parseInt(requestedTestId);

	if (!ready)
		return (<Loading />);

	if (requestedTestId) {

		if (!selectedTest) {
			if (isNaN(testId)) // Id is not a number.
				return (<Error404Page />); // Not found.

			if (requestedStudentImposterId) {
				// Requested imposter, got no test.

				return <Error400Page />;
			}

			// We didn't try imposter, try get normal test view.
			setSelectedTest(testsWithStudentInfo.find(t => t.test.id === testId));

			if (!selectedTest) // Id does not lead to a real test.
				return (<Error404Page />); // Not found.
		}

		return <TakeTest testWithStudentInfo={selectedTest} />;
	}

	return <ListTests testsWithStudentInfo={testsWithStudentInfo} />;
}
