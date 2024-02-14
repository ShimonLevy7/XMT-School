using Microsoft.AspNetCore.Mvc;

using XmtSchoolTypes.Users;
using XmtSchoolDatabase;
using static XmtSchoolWebApi.Types.BaseApi;
using static XmtSchoolWebApi.Types.ResultTypes.UsersResultTypes;
using static XmtSchoolWebApi.Types.BodyTypes.UsersBodyTypes;

namespace XmtSchoolWebApi.Controllers
{
    public class UsersController : Controller
	{
		private readonly ILogger<UsersController> _logger;

		private readonly XmtSchoolDbContext _xmtSchoolDbContext;

		public UsersController(ILogger<UsersController> logger, XmtSchoolDbContext tokensDbContext)
		{
			_logger = logger;
			_xmtSchoolDbContext = tokensDbContext;
		}

		[HttpGet("GetUserByTokenString")]
		public IActionResult GetUserByTokenString([FromHeader] string? TokenString)
		{
			_logger.LogInformation(nameof(GetUserByTokenString) + " was called.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			if (string.IsNullOrEmpty(tokenString))
				return BadRequest();

			// See if the user by token exists.
			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User requestingUser)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			// Return only important user data.
			return Ok(new GetUserResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = new GetUserResultData
				{
					Id = requestingUser.Id,
					Username = requestingUser.Username,
					AvatarUrl = requestingUser.AvatarUrl,
					Email = requestingUser.Email,
					Type = requestingUser.Type
				}
			});
		}

		[HttpGet("GetUserUsername")]
		public IActionResult GetUserUsername([FromHeader] string? TokenString, [FromHeader] int UserId)
		{
			_logger.LogInformation(nameof(GetUserUsername) + " was called.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			if (string.IsNullOrEmpty(tokenString))
				return BadRequest();

			// See if the user by token exists.
			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User requestingUser)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			// Check if user is authorised.
			if (requestingUser.Type <= UserTypes.Guest)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			// Check if the user requested exists.
			User? requestedUser = _xmtSchoolDbContext.GetUserById(UserId);

			if (requestedUser == null)
				return Problem(Utils.GetProblem(ProblemTypes.UserByIdNotFound));

			return Ok(new GetUserUsernameResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = new GetUserUsernameResultData
				{
					Username = requestedUser.Username
				}
			});
		}

		[HttpGet("GetUsersWithSensitiveData")]
		public IActionResult GetAllUsersWithSensitiveData([FromHeader] string? TokenString)
		{
			_logger.LogInformation(nameof(GetAllUsersWithSensitiveData) + " was called.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			if (string.IsNullOrEmpty(tokenString))
				return BadRequest();

			// See if the user by token exists.
			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User requestingUser)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			// Check if user is authorised.
			if (requestingUser.Type < UserTypes.Administrator)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			// Get a list of the users but exclude all password hashes.
			List<User> usersWithoutPasswordHash = new List<User>();

			foreach (User user in _xmtSchoolDbContext.Users.ToList())
			{
				user.Password = string.Empty;

				usersWithoutPasswordHash.Add(user);
			}
			
			return Ok(new GetUsersWithSensitiveDataResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = new GetUsersWithSensitiveDataResultData
				{
					Users = usersWithoutPasswordHash
				}
			});
		}

		[HttpPost("AddUser")]
		public IActionResult AddUser([FromBody] AddUserBody Body)
		{
			_logger.LogInformation(nameof(AddUser) + " was called.");

			if (Body == null)
				return Problem();

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? Body.TokenString ?? string.Empty;

			if (string.IsNullOrEmpty(tokenString))
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			// See if the user by token exists.
			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User requestingUser)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			// Check if user is authorised.
			if (requestingUser.Type < UserTypes.Administrator)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			if (Body.User == null)
				return BadRequest();

			// Ensure no user with a similiar username exists.
			if (_xmtSchoolDbContext.Users.Where(u => u.Username.ToLower() == Body.User.Username.ToLower()).FirstOrDefault() != null)
				return Problem(Utils.GetProblem(ProblemTypes.UsernameUsed));

			try
			{
				Body.User.SanityCheck();
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}

			User userToAdd = Body.User;

			// Check that the user level specified makes sense.
			if (userToAdd.Type > requestingUser.Type)
				return Problem(Utils.GetProblem(ProblemTypes.CannotAddUserWithHigherTypeThanOwn));

			userToAdd.Id = 0; // Id is set by the database.
			userToAdd.Password = Utils.GetMD5HashFromPassword(userToAdd.Password); // Hash the password for the database.

			_xmtSchoolDbContext.AddUser(userToAdd);

			return Ok(new AddUserResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = new AddUserResultData
				{
					UserId = userToAdd.Id
				}
			});
		}

		[HttpDelete("RemoveUser")]
		public IActionResult RemoveUser([FromHeader] string TokenString, [FromHeader] int UserId)
		{
			_logger.LogInformation(nameof(RemoveUser) + " was called.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			// See if the user by token exists.
			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User requestingUser)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			// Check that user is authorised.
			if (requestingUser.Type < UserTypes.Administrator)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			// Find the user to remove.
			if (_xmtSchoolDbContext.Users.Find(UserId) is not User userToRemove)
				return Problem(Utils.GetProblem(ProblemTypes.UserByIdNotFound));

			// Cannot remove self.
			if (userToRemove.Id == requestingUser.Id)
				return Problem(Utils.GetProblem(ProblemTypes.CannotRemoveSelf));

			_xmtSchoolDbContext.Users.Remove(userToRemove);
			_xmtSchoolDbContext.SaveChanges();

			return Ok(new RemoveUserResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
			});
		}

		[HttpPut("UpdateUser")]
		public IActionResult UpdateUser([FromBody] UpdateUserBody Body)
		{
			_logger.LogInformation(nameof(UpdateUser) + " was called.");

			if (Body == null)
				return BadRequest("Body was not sent.");

			if (Body.User == null)
				return BadRequest("Body.User was not sent.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? Body.TokenString ?? string.Empty;

			// See if the user by token exists.
			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User requestingUser)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			// Check that user is authorised.
			if (requestingUser.Type < UserTypes.Administrator)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			// Find user to update.
			if (_xmtSchoolDbContext.Users.Find(Body.User.Id) is not User userInDb)
				return Problem(Utils.GetProblem(ProblemTypes.UserByIdNotFound));

			Body.User.Password = string.IsNullOrWhiteSpace(Body.User.Password) // Was the user sent to update with a password?
				? userInDb.Password // No, set the password from the Db (default).
				: Utils.GetMD5HashFromPassword(Body.User.Password); // Yes, convert to hash.

			if (_xmtSchoolDbContext.Users.Where(u => u.Username == Body.User.Username).Where(u => u.Id != Body.User.Id).FirstOrDefault() != null)
				return Problem(Utils.GetProblem(ProblemTypes.UsernameUsed));

			try
			{
				Body.User.SanityCheck();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			if (Body.User.Id == requestingUser.Id // User to update is the user requesting?
				&& Body.User.Type != requestingUser.Type) // The type is different?
			{
				return Problem(Utils.GetProblem(ProblemTypes.CannotUpdateOwnType)); // Can't change own type!
			}

			userInDb.Username = Body.User.Username;
			userInDb.Password = Body.User.Password;
			userInDb.Email = Body.User.Email;
			userInDb.Type = Body.User.Type;
			userInDb.AvatarUrl = Body.User.AvatarUrl;

			_xmtSchoolDbContext.SaveChanges();

			return Ok(new UpdateUserResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!")
			});
		}
	}
}
