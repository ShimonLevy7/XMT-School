export enum ServerMessageTypes
{
	Info,
	Error,
	Warning
}

export interface IServerMessage
{
	messageType: number;
	messageId: number;
	message: string;
}

export interface IServerErrorResponse
{
	type: string;
	title: string;
	status: number;
	detail: string;
	traceId: string;
}

export interface IServerSuccessResponse
{
	message: IServerMessage;
	data: object;
}

export interface IAPIRequestType
{
	method: string;
	path: string;
}

export interface ITest
{
	id: number;
	name: string;
	creationDate: Date;
	authorUserId: number;
	startDate: Date;
	endDate: Date;
	randomiseQuestionsOrder: boolean;
	randomiseAnswersOrder: boolean;
	questions: IQuestion[];
}

export interface ITestWithStudentInfo
{
	test: ITest;
	authorName: string; // Difference between ITestWithStudentInfo and ITest
	wasTaken: boolean;
	canTake: boolean;
	mark: IMark;
}

export class SubmittableTest
{
	testId: number;
	answers: SelectedAnswer[];

	constructor(testId: number, answers: SelectedAnswer[])
	{
		this.testId = testId;
		this.answers = answers;
	}
}

export interface IQuestion
{
	id: number;
	testId: number;
	questionText: string;
	answers: IAnswer[];
}

export interface IAnswer
{
	id: number;
	questionId: number;
	isValidAnswer: boolean;
	answerText: string;
}

export class SelectedAnswer
{
	questionId: number;
	answerId: number;

	constructor(questionId: number, answerId: number)
	{
		this.questionId = questionId;
		this.answerId = answerId;
	}
}

export interface IMark
{
	id: number;
	userId: number;
	testId: number;
	points: number;
	selectedAnswers: SelectedAnswer[];
}

export const APIRequestTypes: { [key: string]: IAPIRequestType } =
{
	login: { method: 'POST', path: 'Login' },
	logout: { method: 'POST', path: 'Logout' },
	getUserByTokenString: { method: 'GET', path: 'GetUserByTokenString' },
	getStudentMarks: { method: 'GET', path: 'GetStudentMarks' },
	getTestsWithAuthorNameAndStudentInfo: { method: 'GET', path: 'GetTestsWithAuthorNameAndStudentInfo' },
	getTestWithSensitiveData: { method: 'GET', path: 'GetTestWithSensitiveData' },
	getUserUsername: { method: 'GET', path: 'GetUserUsername' },
	submitTest: { method: 'POST', path: 'SubmitTest' },
	getTestWithAuthorNameAndStudentInfo: { method: 'GET', path: 'GetTestWithAuthorNameAndStudentInfo' }
};

////////////////////////////////////////////////////////////////////////////////////////////////////
// Get
////////////////////////////////////////////////////////////////////////////////////////////////////

export interface IUserResult extends IServerSuccessResponse
{
	message: IServerMessage;
	data: {
		id: number,
		username: string,
		avatarUrl: string | undefined,
		email: string,
		type: number
	};
}

export interface IStudentMarksResult extends IServerSuccessResponse
{
	message: IServerMessage;
	data: IMark[];
}

export interface IGetUndisclosedTestsResult extends IServerSuccessResponse
{
	message: IServerMessage;
	data: ITest[];
}

export interface IgetUndisclosedTestsWithStudentInfoResult extends IServerSuccessResponse
{
	message: IServerMessage;
	data: ITestWithStudentInfo[];
}

export interface IGetDisclosedTestResult extends IServerSuccessResponse
{
	message: IServerMessage;
	data: {
		test: ITest
	};
}

export interface GetTestWithAuthorNameAndStudentInfoResult extends IServerSuccessResponse
{
	message: IServerMessage;
	data: ITestWithStudentInfo;
}

export interface IUserUsernameResult extends IServerSuccessResponse
{
	message: IServerMessage;
	data: {
		username: string
	};
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// Post
////////////////////////////////////////////////////////////////////////////////////////////////////

export interface ILoginResult extends IServerSuccessResponse {
	message: IServerMessage;
	data: {
		tokenString: string
	};
}

export interface ILogoutResult extends IServerSuccessResponse {
	message: IServerMessage;
}

export interface ISubmitTestResult extends IServerSuccessResponse
{
	message: IServerMessage;
	data: {
		mark: IMark
	};
}
