namespace XmtSchoolWebApi.Types
{
    public static class BaseApi
    {
        public enum ServerMessageTypes : byte
        {
            Info,
            Error,
            Warning,
            Success
        }

        public enum ProblemTypes : ushort
        {
            InvalidLoginCredentials = 1000,
            UserByTokenNotFound = 1001,
            StudentByTokenNotFound = 1002,
            UserByTokenNotAuthorised = 1003,
            UserByIdNotFound = 1004,
            StudentAlreadyTookTest = 1005,
            TestNotFound = 1006,
            InvalidTestSubmitted = 1007,
            StudentHasNotTakenThisTest = 1008,
            InvalidTestUploaded = 1009,
            TestNotStarted = 1010,
            InvalidUser = 1011,
			CannotRemoveSelf = 1012,
			CannotUpdateOwnType = 1013,
			CannotAddUserWithHigherTypeThanOwn = 1014,
			UsernameUsed = 1015,
			MarkByIdNotFound = 1016
		}

        public static readonly Dictionary<ProblemTypes, string> ProblemTypeToString = new Dictionary<ProblemTypes, string>()
        {
            [ProblemTypes.InvalidLoginCredentials] = "Invalid username or password.",
            [ProblemTypes.UserByTokenNotFound] = "User with matching token not found.",
            [ProblemTypes.StudentByTokenNotFound] = "Student user with matching token not found.",
            [ProblemTypes.UserByTokenNotAuthorised] = "User with specified token is not authorised to access this data or do this action.",
            [ProblemTypes.UserByIdNotFound] = "User with specified ID was not found.",
            [ProblemTypes.StudentAlreadyTookTest] = "Student already took this test.",
            [ProblemTypes.TestNotFound] = "Test by specified ID was not found.",
            [ProblemTypes.InvalidTestSubmitted] = "The test submitted is invalid.",
            [ProblemTypes.StudentHasNotTakenThisTest] = "The student specified has not taken this test before.",
            [ProblemTypes.InvalidTestUploaded] = "Invalid test uploaded (error: {0}).",
            [ProblemTypes.TestNotStarted] = "Test has not started yet.",
            [ProblemTypes.InvalidUser] = "Invalid user.",
            [ProblemTypes.CannotRemoveSelf] = "You cannot remove your own user.\nThis is a security measure.\n\nContact the IT department for more information.",
            [ProblemTypes.CannotUpdateOwnType] = "You cannot change the type of your own user.\nThis is a security measure.\n\nContact the IT department for more information.",
            [ProblemTypes.CannotAddUserWithHigherTypeThanOwn] = "You cannot add a user with a user type that is higher than your own.",
            [ProblemTypes.UsernameUsed] = "Another user was found with the same username.",
            [ProblemTypes.MarkByIdNotFound] = "Mark with specified ID was not found."
		};

        public record ServerMessage(ServerMessageTypes MessageType, string Message);

        public interface IServerResponse
        {
            ServerMessage Message { get; set; }
        }
    }
}
