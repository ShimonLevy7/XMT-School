import { IAPIRequestType, IServerErrorResponse } from "./Types";
import { HandleAPIError, InDevMode } from "./Utils";

export const ApiUrl: string = InDevMode() ? "http://localhost:50000/" : "https://api.tiro-finale.com/";

export async function PostAsync<ResultType>(RequestTypes: IAPIRequestType, Body: object)
{
	try
	{
		const response = await fetch(ApiUrl + RequestTypes.path, {
			method: RequestTypes.method,
			mode: 'cors',
			headers: {
				'Content-Type': 'application/json',
				'Accept': 'application/json'
			},
			body: JSON.stringify(Body),
			credentials: 'include'
		});

		if (!response.ok)
		{
			const result: IServerErrorResponse = (await response.json()) as IServerErrorResponse;

			if (result && result.detail)
				throw new Error(result.detail);

			throw new Error(`Unhandled Server Error (Status ${response.status})`);
		}

		const result = (await response.json()) as ResultType;

		return result;
	}
	catch (error)
	{
		return HandleAPIError(error as Error);
	}
}

export async function GetAsync<ResultType>(RequestTypes: IAPIRequestType, Headers: HeadersInit | undefined)
{
	try
	{
		const response = await fetch(ApiUrl + RequestTypes.path, {
			method: RequestTypes.method,
			mode: 'cors',
			headers: Headers,
			credentials: 'include'
		});

		if (!response.ok)
		{
			const result: IServerErrorResponse = (await response.json()) as IServerErrorResponse;

			if (result && result.detail)
				throw new Error(result.detail);

			throw new Error(`Unhandled Server Error (Status ${response.status})`);
		}

		const result = (await response.json()) as ResultType;

		return result;
	}
	catch (error)
	{
		return HandleAPIError(error as Error);
	}
}
