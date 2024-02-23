using XmtSchoolTypes.Users;

namespace XmtSchoolWebApi.Types.BodyTypes
{
	public class UsersBodyTypes
	{
        public record AddUserBody // POST body.
        {
            public string TokenString { get; set; }
            public User? User { get; set; }

            public AddUserBody(string TokenString, User? User)
            {
                this.TokenString = TokenString;
				this.User = User;
            }
        }

		public record UpdateUserBody
		{
			public string? TokenString { get; set; }
            public User? User { get; set; }

            public UpdateUserBody(string TokenString, User? User)
            {
                this.TokenString = TokenString;
                this.User = User;
            }
        }
	}
}
