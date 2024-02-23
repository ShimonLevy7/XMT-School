using static XmtSchoolWebApi.Types.BaseApi;
using XmtSchoolTypes.Tests;

namespace XmtSchoolWebApi.Types.ResultTypes
{
	public class TestsResultTypes
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////
		// GET
		////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Get all tests (without sensitive data like answers)
		/// </summary>
		public class GetTestsResult : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
			public List<Test> Data { get; set; } = new List<Test>();
		}

		/// <summary>
		/// Get all tests (without sensitive data like answers) - include author name
		/// </summary>
		public class GetTestsWithAuthorNameAndStudentInfo : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
			public List<TestWithAuthorNameAndStudentInfo> Data { get; set; } = new List<TestWithAuthorNameAndStudentInfo>();
		}

		/// <summary>
		/// Get all tests with all data & author name
		/// </summary>
		public class GetTestsWithSensitiveDataResultData
		{
			public List<TestWithAuthorName>? Tests { get; set; } = null;
		}

		public class GetTestsWithSensitiveDataResult : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
			public GetTestsWithSensitiveDataResultData Data { get; set; } = new GetTestsWithSensitiveDataResultData();
		}

		/// <summary>
		/// Get a single disclosed (including all data) test
		/// </summary>
		public class GetTestWithSensitiveDataResultData
		{
			public Test? Test { get; set; } = null;
		}

		public class GetTestWithSensitiveDataResult : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
			public GetTestWithSensitiveDataResultData Data { get; set; } = new GetTestWithSensitiveDataResultData();
		}

		/// <summary>
		/// Get all tests with student info
		/// </summary>
		public class GetDisclosedTestsWithStudentInfo
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
			public List<TestWithAuthorNameAndStudentInfo> Data { get; set; } = new List<TestWithAuthorNameAndStudentInfo>();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		// POST
		////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Add Test
		/// </summary>
		public class AddTestResult : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
		}

		/// <summary>
		/// Submit test after taking it
		/// </summary>
		public class SubmitTestResultData
		{
			public Mark Mark { get; set; } = new Mark(-1, -1, -1, -1, new List<SelectedAnswer>());
		}

		public class SubmitTestResult : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
			public SubmitTestResultData Data { get; set; } = new SubmitTestResultData();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		// DELETE
		////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Add Test
		/// </summary>
		public class RemoveTestResult : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		// PUT
		////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Add Test
		/// </summary>
		public class UpdateTestResult : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
		}
    }
}
