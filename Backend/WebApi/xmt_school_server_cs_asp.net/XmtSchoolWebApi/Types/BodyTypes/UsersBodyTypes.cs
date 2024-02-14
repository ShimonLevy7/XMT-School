using XmtSchoolTypes.Users;

namespace XmtSchoolWebApi.Types.BodyTypes
{
	public class UsersBodyTypes
	{
		public record AddUserBody
		{
			public string? TokenString { get; set; } = string.Empty;
			public User? User { get; set; } = null;
		}

		public record RemoveUserBody
		{
			public string? TokenString { get; set; } = string.Empty;
			public int UserId { get; set; } = 0;
		}

		public record UpdateUserBody
		{
			public string? TokenString { get; set; } = string.Empty;
			public User? User { get; set; } = null;
		}
	}
}
