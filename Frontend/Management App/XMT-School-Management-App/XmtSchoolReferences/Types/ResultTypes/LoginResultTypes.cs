using static XmtSchoolWebApi.Types.BaseApi;

namespace XmtSchoolWebApi.Types.ResultTypes
{
	public class LoginResultTypes
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////
		// Post
		////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Login
		/// </summary>
		public class LoginResultData
		{
			public string? TokenString { get; set; } = null;
		}

		public class LoginResult : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
			public LoginResultData Data { get; set; } = new LoginResultData();
		}

		/// <summary>
		/// Logout
		/// </summary>
		public class LogoutResult : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
		}
	}
}
