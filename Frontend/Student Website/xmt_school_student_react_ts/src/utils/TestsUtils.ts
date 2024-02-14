import { ITestWithStudentInfo } from "../api/Types";

export const goToTestUrl = function(testsWithStudentInfo: ITestWithStudentInfo)
{
	const params = new URLSearchParams();

	params.append('testId', testsWithStudentInfo.test.id.toString())

	const url = new URL(window.location.href);

	url.search = params.toString();

	window.location.href = url.toString();
}
