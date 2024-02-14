import { PostAsync } from "./API";
import { APIRequestTypes, ILoginResult, ILogoutResult } from "./Types";

export async function loginAsync(username: string, password: string)
{

	return await PostAsync<ILoginResult>(APIRequestTypes.login, {
		Username: username,
		Password: password
	});
}

export async function logoutAsync(userId: number)
{

	return await PostAsync<ILogoutResult>(APIRequestTypes.logout, {
		UserId: userId.toString()
	});
}
