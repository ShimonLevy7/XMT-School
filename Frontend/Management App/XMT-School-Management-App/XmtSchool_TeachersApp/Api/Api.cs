using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using XmtSchool_TeachersApp.Cache;
using XmtSchoolTypes.Tests;
using XmtSchoolTypes.Users;
using static XmtSchoolWebApi.Types.BodyTypes.TestsBodyTypes;
using static XmtSchoolWebApi.Types.BodyTypes.UsersBodyTypes;
using static XmtSchoolWebApi.Types.ResultTypes.LoginResultTypes;
using static XmtSchoolWebApi.Types.ResultTypes.MarksResultTypes;
using static XmtSchoolWebApi.Types.ResultTypes.TestsResultTypes;
using static XmtSchoolWebApi.Types.ResultTypes.UsersResultTypes;

namespace XmtSchool_TeachersApp.Api
{
	public static class Api
	{
        public const string ApiUrl = "https://api.tiro-finale.com/"; // "http://localhost:50000/"

        public struct ServerErrorResponse
		{
			public string Type { get; set; }
			public string Title { get; set; }
			public ushort Status { get; set; }
			public string Detail { get; set; }
			public string TraceId { get; set; }
		}

		public struct RequestInfo
		{
			public HttpMethod Method { get; private set; }
			public string Path { get; private set; }

			public RequestInfo(HttpMethod Method, string Path)
			{
				this.Method = Method;
				this.Path = Path;
			}
		}

		public readonly struct GetResult
		{
			public bool Success { get; }
			public object? Data { get; }

			public GetResult(bool Success, object? Data)
			{
				this.Success = Success;
				this.Data = Data;
			}
		}

		public enum RequestTypes
		{
			Login,
			Logout,
			GetUserByTokenString,
			GetStudentMarks,
            GetUserUsername,
			SubmitTest,
            GetUsersWithSensitiveData,
            GetTestsWithSensitiveData,
            AddUser,
            AddTest,
            UpdateUser,
            UpdateTest,
            RemoveUser,
			RemoveTest,
            GetAllMarks,
            RemoveMark
        }

		public static readonly Dictionary<RequestTypes, RequestInfo> ApiRequest = new()
		{
			[RequestTypes.Login] = new RequestInfo(Method: HttpMethod.Post, Path: "Login"),
			[RequestTypes.Logout] = new RequestInfo(Method: HttpMethod.Post, Path: "Logout"),
			[RequestTypes.GetUserByTokenString] = new RequestInfo(Method: HttpMethod.Get, Path: "GetUserByTokenString"),
			[RequestTypes.GetStudentMarks] = new RequestInfo(Method: HttpMethod.Get, Path: "GetStudentMarks"),
            [RequestTypes.GetUserUsername] = new RequestInfo(Method: HttpMethod.Get, Path: "GetUserUsername"),
			[RequestTypes.SubmitTest] = new RequestInfo(Method: HttpMethod.Post, Path: "SubmitTest"),
            [RequestTypes.GetUsersWithSensitiveData] = new RequestInfo(Method: HttpMethod.Get, Path: "GetUsersWithSensitiveData"),
			[RequestTypes.GetTestsWithSensitiveData] = new RequestInfo(Method: HttpMethod.Get, Path: "GetTestsWithSensitiveData"),
			[RequestTypes.AddUser] = new RequestInfo(Method: HttpMethod.Post, Path: "AddUser"),
            [RequestTypes.AddTest] = new RequestInfo(Method: HttpMethod.Post, Path: "AddTest"),
            [RequestTypes.UpdateUser] = new RequestInfo(Method: HttpMethod.Put, Path: "UpdateUser"),
            [RequestTypes.UpdateTest] = new RequestInfo(Method: HttpMethod.Put, Path: "UpdateTest"),
            [RequestTypes.RemoveUser] = new RequestInfo(Method: HttpMethod.Delete, Path: "RemoveUser"),
            [RequestTypes.RemoveTest] = new RequestInfo(Method: HttpMethod.Delete, Path: "RemoveTest"),
            [RequestTypes.GetAllMarks] = new RequestInfo(Method: HttpMethod.Get, Path: "GetAllMarks"),
            [RequestTypes.RemoveMark] = new RequestInfo(Method: HttpMethod.Delete, Path: "RemoveMark")
        };

		private static async Task<GetResult> RequestAsync<T>(HttpMethod Method, string ApiPath, Dictionary<string, string>? RequestHeaders = null, object? RequestBody = null)
		{
			using (HttpClient client = new())
			{
				try
				{
					// Set the headers
					client.DefaultRequestHeaders.Add("Accept", "application/json");
					// client.DefaultRequestHeaders.Add("Content-Type", "application/json"); // This is bad!!!

					if (RequestHeaders != null)
					{
						foreach (KeyValuePair<string, string> headerKeyValue in RequestHeaders)
							client.DefaultRequestHeaders.Add(headerKeyValue.Key, headerKeyValue.Value);
					}

					// Send a POST request to the API endpoint
					HttpResponseMessage response;

					// Declare endpoint.
					Uri endPoint = new Uri(string.Concat(ApiUrl, ApiPath));

					// Declare request.
                    HttpRequestMessage request = new HttpRequestMessage(Method, endPoint);

					// Add a body for matching method types.
					if (Method != HttpMethod.Get && Method != HttpMethod.Delete && Method != HttpMethod.Head)
                    {
                        // Serialize the request body
                        string requestBodyJson = JsonConvert.SerializeObject(RequestBody);

                        // Create the request content
                        StringContent content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                        request.Content = content;
                    }

                    response = await client.SendAsync(request);

					if (response.IsSuccessStatusCode)
					{
						// Successful request, receive the LoginResult from the API
						string resultJson = await response.Content.ReadAsStringAsync();
						T? requestResult = JsonConvert.DeserializeObject<T>(resultJson);

						return new GetResult(true, requestResult);
					}
					else
					{
						// Incorrect request, receive the error message from the API
						string error = await response.Content.ReadAsStringAsync();

						return new GetResult(false, JsonConvert.DeserializeObject<ServerErrorResponse>(error));
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred: {ex.Message}");
				}
			}

			return new GetResult(false, null);
		}

		public record LoginBody // POST body.
		{
			public string Username { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;

			public LoginBody(string Username, string Password)
			{
				this.Username = Username;
				this.Password = Password;
			}
		}

		public static async Task<bool> LoginAsync(string Username, string Password)
		{
			RequestInfo requestInfo = ApiRequest[RequestTypes.Login];
			GetResult result = await RequestAsync<LoginResult>(requestInfo.Method, requestInfo.Path, null, new LoginBody(Username, Password));

			if (result.Success && result.Data is LoginResult loginResult)
			{
				if (string.IsNullOrEmpty(loginResult.Data.TokenString))
					return false;

				CurrentToken.TokenString = loginResult.Data.TokenString; // Store the token in the static property

				return true;
			}

			if (result.Data == null)
			{
				MessageBox.Show("Unknown error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

				return false;
			}

			if (!result.Success && result.Data is ServerErrorResponse serverError)
			{
				MessageBox.Show(serverError.Detail, serverError.Title, MessageBoxButton.OK, MessageBoxImage.Error);

				return false;
			}

			return false;
		}

		// Get.
		public static async Task<GetUserResultData?> GetUserByTokenStringAsync()
		{
			RequestInfo requestInfo = ApiRequest[RequestTypes.GetUserByTokenString];
			GetResult result = await RequestAsync<GetUserResult>(requestInfo.Method, requestInfo.Path, RequestHeaders: new Dictionary<string, string>()
			{
				["TokenString"] = CurrentToken.TokenString
			});

			if (result.Success && result.Data is GetUserResult getUserByTokenStringResult)
				return getUserByTokenStringResult.Data;

			return null;
		}

        public static async Task<GetUsersWithSensitiveDataResult?> GetAllUsersWithSensitiveDataAsync()
        {
            RequestInfo requestInfo = ApiRequest[RequestTypes.GetUsersWithSensitiveData];
            GetResult result = await RequestAsync<GetUsersWithSensitiveDataResult>(requestInfo.Method, requestInfo.Path, RequestHeaders: new Dictionary<string, string>()
            {
                ["TokenString"] = CurrentToken.TokenString
            });

            if (result.Success && result.Data is GetUsersWithSensitiveDataResult getUsersResult)
            {
                return getUsersResult;
            }

            return null;
        }

        public static async Task<GetTestsWithSensitiveDataResult?> GetDisclosedTestsAsync()
        {
            RequestInfo requestInfo = ApiRequest[RequestTypes.GetTestsWithSensitiveData];
            GetResult result = await RequestAsync<GetTestsWithSensitiveDataResult>(requestInfo.Method, requestInfo.Path, RequestHeaders: new Dictionary<string, string>()
            {
                ["TokenString"] = CurrentToken.TokenString
            });

            if (result.Success && result.Data is GetTestsWithSensitiveDataResult getTestsResult)
            {
                return getTestsResult;
            }

            return null;
        }

        public static async Task<GetAllMarksResult?> GetAllMarksAsync()
        {
            RequestInfo requestInfo = ApiRequest[RequestTypes.GetAllMarks];
            GetResult result = await RequestAsync<GetAllMarksResult>(requestInfo.Method, requestInfo.Path, RequestHeaders: new Dictionary<string, string>()
            {
                ["TokenString"] = CurrentToken.TokenString
            });

            if (result.Success && result.Data is GetAllMarksResult getAllMarksResult)
                return getAllMarksResult;

            if (result.Data is ServerErrorResponse serverError)
            {
                MessageBox.Show(serverError.Detail, serverError.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        // Post.
        public static async Task<AddUserResult?> AddUserAsync(User user)
        {
            RequestInfo requestInfo = ApiRequest[RequestTypes.AddUser];
			GetResult result = await RequestAsync<AddUserResult>(requestInfo.Method, requestInfo.Path, null, new AddUserBody(CurrentToken.TokenString, user));

            if (result.Success && result.Data is AddUserResult addUserResult)
					return addUserResult;

            if (result.Data is ServerErrorResponse serverError)
            {
                MessageBox.Show(serverError.Detail, serverError.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        public static async Task<AddTestResult?> AddTestAsync(Test test)
        {
            RequestInfo requestInfo = ApiRequest[RequestTypes.AddTest];
            GetResult result = await RequestAsync<AddTestResult>(requestInfo.Method, requestInfo.Path, null, new AddTestBody(CurrentToken.TokenString, test));

            if (result.Success && result.Data is AddTestResult addTestResult)
                return addTestResult;

            if (result.Data is ServerErrorResponse serverError)
            {
                MessageBox.Show(serverError.Detail, serverError.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        // Delete.
        public static async Task<RemoveUserResult?> RemoveUserAsync(int userId)
        {
            RequestInfo requestInfo = ApiRequest[RequestTypes.RemoveUser];
            GetResult result = await RequestAsync<RemoveUserResult>(requestInfo.Method, requestInfo.Path, RequestHeaders: new Dictionary<string, string>()
            {
                ["TokenString"] = CurrentToken.TokenString,
				["UserId"] = userId.ToString() // API is smart enough to convert.
            });

            if (result.Success && result.Data is RemoveUserResult removeUserResult)
                return removeUserResult;

            if (result.Data is ServerErrorResponse serverError)
            {
                MessageBox.Show(serverError.Detail, serverError.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        // Put.
        public static async Task<UpdateUserResult?> UpdateUserAsync(User user)
        {
            RequestInfo requestInfo = ApiRequest[RequestTypes.UpdateUser];
            GetResult result = await RequestAsync<UpdateUserResult>(requestInfo.Method, requestInfo.Path, null, new UpdateUserBody(CurrentToken.TokenString, user));

            if (result.Data is ServerErrorResponse serverError)
            {
                MessageBox.Show(serverError.Detail, serverError.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return result.Success ? (UpdateUserResult?)result.Data : null;
        }

        public static async Task<UpdateTestResult?> UpdateTestAsync(Test test)
        {
            if (test == null)
                throw new ArgumentNullException(nameof(test));

            if (!ApiRequest.TryGetValue(RequestTypes.UpdateTest, out RequestInfo value))
                throw new KeyNotFoundException($"{RequestTypes.UpdateTest} is not found in the API request dictionary.");

            RequestInfo requestInfo = value;

            // Assuming your UpdateTestBody Constructor accepts a Token string and a Test object
            UpdateTestBody requestBody = new UpdateTestBody(CurrentToken.TokenString, test);

            GetResult result;

            try
            {
                // Updating the RequestAsync method call by passing the requestBody
                result = await RequestAsync<UpdateTestResult>(requestInfo.Method, requestInfo.Path, null, requestBody);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred during the request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return null;
            }

            if (result.Data is ServerErrorResponse serverError)
            {
                MessageBox.Show(serverError.Detail, serverError.Title, MessageBoxButton.OK, MessageBoxImage.Error);

                return null;
            }

            return (UpdateTestResult?)result.Data;
        }

        public static async Task<RemoveTestResult?> RemoveTestAsync(int testId)
        {
            RequestInfo requestInfo = ApiRequest[RequestTypes.RemoveTest];
            GetResult result = await RequestAsync<RemoveTestResult>(requestInfo.Method, requestInfo.Path, RequestHeaders: new Dictionary<string, string>()
            {
                ["TokenString"] = CurrentToken.TokenString,
                ["TestId"] = testId.ToString() // API is smart enough to convert.
            });

            if (result.Success && result.Data is RemoveTestResult removeTestResult)
                return removeTestResult;

            if (result.Data is ServerErrorResponse serverError)
            {
                MessageBox.Show(serverError.Detail, serverError.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        public static async Task<RemoveMarkResult?> RemoveMarkAsync(int markId)
        {
            RequestInfo requestInfo = ApiRequest[RequestTypes.RemoveMark];
            GetResult result = await RequestAsync<RemoveMarkResult>(requestInfo.Method, requestInfo.Path, RequestHeaders: new Dictionary<string, string>()
            {
                ["TokenString"] = CurrentToken.TokenString,
                ["MarkId"] = markId.ToString() // API is smart enough to convert.
            });

            if (result.Success && result.Data is RemoveMarkResult removeMarkResult)
                return removeMarkResult;

            if (result.Data is ServerErrorResponse serverError)
            {
                MessageBox.Show(serverError.Detail, serverError.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }
    }
}
