using Microsoft.AspNetCore.Mvc;

using XmtSchoolDatabase;

using XmtSchoolTypes.Login;
using XmtSchoolTypes.Users;

using static XmtSchoolWebApi.Types.BaseApi;
using static XmtSchoolWebApi.Types.ResultTypes.LoginResultTypes;

namespace XmtSchoolWebApi.Controllers
{
    public class LoginController : Controller
	{
		private readonly ILogger<LoginController> _logger;

		private readonly XmtSchoolDbContext _xmtSchoolDbContext;

		public LoginController(ILogger<LoginController> logger, XmtSchoolDbContext tokensDbContext)
		{
			_logger = logger;
			_xmtSchoolDbContext = tokensDbContext;
		}

		public record LoginBody
		{
			public string Username { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
		}

		[HttpPost("Login")]
		public IActionResult Login([FromBody] LoginBody Body)
		{
			_logger.LogInformation(nameof(Login) + " was called.");

			if (Body == null)
				return BadRequest();

			Body.Username = Body.Username.ToLower().Trim();
			Body.Password = Utils.GetMD5HashFromPassword(Body.Password); // Hash the password.

			User? user = _xmtSchoolDbContext.GetUserByUsernameAndPassword(Body.Username, Body.Password); // Try to find the user.

			if (user == null)
				return Problem(Utils.GetProblem(ProblemTypes.InvalidLoginCredentials));

			Token token = new Token(user.Id); // Create a token.

			_xmtSchoolDbContext.AddToken(token); // Save the token in the database.

			return Ok(new LoginResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = new LoginResultData { TokenString = token.TokenString } // Return just the token string.
			});

		}

		public record LogoutBody
		{
			public string? TokenString { get; set; } = string.Empty;
			public uint UserId { get; set; } = 0;
		}

		[HttpPost("Logout")]
		public IActionResult Logout([FromBody] LogoutBody Body)
		{
			_logger.LogInformation(nameof(Logout) + " was called.");

			if (Body == null)
				return BadRequest();

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? Body.TokenString ?? string.Empty;

			// See if the token points at a user.
			if (_xmtSchoolDbContext.FindToken(tokenString, Body.UserId) is not Token token)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound)); // It does not.

			_xmtSchoolDbContext.RemoveToken(token); // Remove the token from the database.

			return Ok(new LogoutResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!")
			});
		}
	}
}
