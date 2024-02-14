using XmtSchoolTypes.Users;

using static XmtSchoolWebApi.Types.BaseApi;

namespace XmtSchoolWebApi.Types.ResultTypes
{
    public static class UsersResultTypes
    {
		////////////////////////////////////////////////////////////////////////////////////////////////////
		// GET
		////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Get User
		/// </summary>
		public class GetUserResultData
        {
            public int Id { get; set; } = 0;
            public string Username { get; set; } = "N/A";
            public string? AvatarUrl { get; set; } = null;
            public string Email { get; set; } = "N/A";
            public UserTypes Type { get; set; } = UserTypes.Guest;
        }

        public class GetUserResult : IServerResponse
        {
            public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
            public GetUserResultData Data { get; set; } = new GetUserResultData();
        }

		/// <summary>
		/// Get User Username
		/// </summary>
		public class GetUserUsernameResultData
        {
            public string Username { get; set; } = "N/A";
        }

        public class GetUserUsernameResult : IServerResponse
        {
            public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
            public GetUserUsernameResultData Data { get; set; } = new GetUserUsernameResultData();
        }

		/// <summary>
		/// Get Get All Users With Sensitive Data
		/// </summary>
		public class GetUsersWithSensitiveDataResultData
        {
            public List<User> Users { get; set; } = new List<User>();
        }

        public class GetUsersWithSensitiveDataResult : IServerResponse
        {
            public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
            public GetUsersWithSensitiveDataResultData Data { get; set; } = new GetUsersWithSensitiveDataResultData();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		// POST
		////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Add User
		/// </summary>
		public class AddUserResultData
		{
			public int UserId { get; set; } = 0;
		}

		public class AddUserResult : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
			public AddUserResultData Data { get; set; } = new AddUserResultData();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		// DELETE
		////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Remove User
		/// </summary>
		public class RemoveUserResult : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		// PUT
		////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Update User
		/// </summary>
		public class UpdateUserResult : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
		}
	}
}
