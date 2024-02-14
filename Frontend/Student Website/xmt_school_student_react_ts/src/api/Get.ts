import { GetAsync } from "./API";
import { APIRequestTypes, IGetDisclosedTestResult, IgetUndisclosedTestsWithStudentInfoResult, IStudentMarksResult, IUserResult } from "./Types";

export async function getUserByTokenAsync(tokenString: string)
{
	if (tokenString === '')
		return 'User not logged in';

	return await GetAsync<IUserResult>(APIRequestTypes.getUserByTokenString, {
		Accept: 'application/json'
	});
}

export async function getStudentMarksAsync(tokenString: string)
{
	if (tokenString === '')
		return 'User not logged in';

	return await GetAsync<IStudentMarksResult>(APIRequestTypes.getStudentMarks, {
		Accept: 'application/json'
	});
}

export async function getTestsWithAuthorNameAndStudentInfoAsync(tokenString: string)
{
	if (tokenString === '')
		return 'User not logged in';

	return await GetAsync<IgetUndisclosedTestsWithStudentInfoResult>(APIRequestTypes.getTestsWithAuthorNameAndStudentInfo, {
		Accept: 'application/json'
	});
}

export async function getTestWithSensitiveDataAsync(tokenString: string, testId: number)
{
	if (tokenString === '')
		return 'User not logged in';

	return await GetAsync<IGetDisclosedTestResult>(APIRequestTypes.getTestWithSensitiveData, {
		Accept: 'application/json',
		TestId: testId.toString()
	});
}
