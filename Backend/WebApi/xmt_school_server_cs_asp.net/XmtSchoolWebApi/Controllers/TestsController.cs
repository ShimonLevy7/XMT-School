using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using XmtSchoolDatabase;

using XmtSchoolTypes.Tests;
using XmtSchoolTypes.Users;

using static XmtSchoolWebApi.Types.BaseApi;
using static XmtSchoolWebApi.Types.BodyTypes.TestsBodyTypes;
using static XmtSchoolWebApi.Types.ResultTypes.MarksResultTypes;
using static XmtSchoolWebApi.Types.ResultTypes.TestsResultTypes;

namespace XmtSchoolWebApi.Controllers
{
    public class TestsController : Controller
	{
		private readonly ILogger<TestsController> _logger;

		private readonly XmtSchoolDbContext _xmtSchoolDbContext;

		public TestsController(ILogger<TestsController> logger, XmtSchoolDbContext tokensDbContext)
		{
			_logger = logger;
			_xmtSchoolDbContext = tokensDbContext;
		}

		[HttpGet("GetStudentMarks")]
		public IActionResult GetStudentMarks([FromHeader] string? TokenString)
		{
			_logger.LogInformation(nameof(GetStudentMarks) + " was called.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			if (string.IsNullOrEmpty(tokenString))
				return BadRequest();

			User? user = _xmtSchoolDbContext.GetUserByTokenString(tokenString);

			if (user == null)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			List<Mark>? marks = _xmtSchoolDbContext.GetMarksForUser(user.Id);

			if (marks == null)
				return Problem(Utils.GetProblem(ProblemTypes.StudentByTokenNotFound));

			return Ok(new GetStudentMarksResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = marks
			});
		}

		[HttpGet("GetAllMarks")]
		public IActionResult GetAllMarks([FromHeader] string? TokenString)
		{
			_logger.LogInformation(nameof(GetAllMarks) + " was called.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			if (string.IsNullOrEmpty(tokenString))
				return BadRequest();

			User? user = _xmtSchoolDbContext.GetUserByTokenString(tokenString);

			if (user == null)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			if (user.Type < UserTypes.Teacher)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			List<Mark> marks = _xmtSchoolDbContext.Marks
				.Include(m => m.SelectedAnswers).ToList();

			return Ok(new GetAllMarksResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = marks
			});
		}

		[HttpDelete("RemoveMark")]
		public IActionResult RemoveMark([FromHeader] string? TokenString, [FromHeader] int MarkId)
		{
			_logger.LogInformation(nameof(RemoveMark) + " was called.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			if (string.IsNullOrEmpty(tokenString))
				return BadRequest();

			User? user = _xmtSchoolDbContext.GetUserByTokenString(tokenString);

			if (user == null)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			if (user.Type < UserTypes.Teacher)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			Mark? markToDelete = _xmtSchoolDbContext.Marks.Where(m => m.Id == MarkId).FirstOrDefault();

			if (markToDelete is null)
				return Problem(Utils.GetProblem(ProblemTypes.MarkByIdNotFound));

			_xmtSchoolDbContext.Marks.Remove(markToDelete);
			_xmtSchoolDbContext.SaveChanges();

			return Ok(new RemoveMarkResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
			});
		}

		[HttpGet("GetTests")]
		public IActionResult GetTests([FromHeader] string? TokenString)
		{
			_logger.LogInformation(nameof(GetTests) + " was called.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			if (string.IsNullOrEmpty(tokenString))
				return BadRequest();

			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User user)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			if (user.Type == UserTypes.Guest)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			List<Test> tests = _xmtSchoolDbContext.GetAllTests(true);
			List<Test> undisclosedTests = new List<Test>();

			foreach (Test test in tests)
				undisclosedTests.Add(test.GetUndisclosedTest(true));

			return Ok(new GetTestsResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = undisclosedTests
			});
		}

		[HttpGet("GetTestsWithAuthorNameAndStudentInfo")]
		public IActionResult GetTestsWithAuthorNameAndStudentInfo([FromHeader] string? TokenString)
		{
			_logger.LogInformation(nameof(GetTestsWithAuthorNameAndStudentInfo) + " was called.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			if (string.IsNullOrEmpty(tokenString))
				return BadRequest();

			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User user)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			if (user.Type == UserTypes.Guest)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			List<Test> tests = _xmtSchoolDbContext.GetAllTests(true);
			List<TestWithAuthorNameAndStudentInfo> undisclosedTests = new List<TestWithAuthorNameAndStudentInfo>();

			foreach (Test test in tests)
			{
				Test newUndisclosedTest = test.GetUndisclosedTest(true);
				string authorUsername = test.Author?.Username ?? "N/A";
				Mark? mark = _xmtSchoolDbContext.GetMarksForUser(user.Id).Where(m => m.TestId == test.Id).FirstOrDefault();
				bool wasTaken = mark != null;
				bool canTake = test.CanBeTaken(wasTaken);

				undisclosedTests.Add(new TestWithAuthorNameAndStudentInfo(newUndisclosedTest, authorUsername, wasTaken, canTake, mark ?? new Mark()));
			}

			return Ok(new GetTestsWithAuthorNameAndStudentInfoResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = undisclosedTests
			});
		}
		
		[HttpGet("GetTestWithAuthorNameAndStudentInfo")]
		public IActionResult GetTestWithAuthorNameAndStudentInfo([FromHeader] string? TokenString, [FromHeader] int? TestId, [FromHeader] int StudentId)
		{
			_logger.LogInformation(nameof(GetTestWithAuthorNameAndStudentInfo) + " was called.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			if (string.IsNullOrEmpty(tokenString))
				return BadRequest();

			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User user)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			if (_xmtSchoolDbContext.GetUserById(StudentId) is null)
				return Problem(Utils.GetProblem(ProblemTypes.UserByIdNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			if (user.Type < UserTypes.Teacher)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			Test? test = _xmtSchoolDbContext.Tests
				.Include(test => test.Author) // Include the author for every Test
				.Include(test => test.Questions) // Include the Questions related to each Test
					.ThenInclude(question => question.Answers) // Include the Answers related to each Question
				.Where(t => t.Id == TestId).FirstOrDefault();

			if (test is null)
				return Problem(Utils.GetProblem(ProblemTypes.TestNotFound));

			TestWithAuthorNameAndStudentInfo testWithData = new TestWithAuthorNameAndStudentInfo();

			Mark? mark = _xmtSchoolDbContext.GetMarksForUser(StudentId).Where(m => m.TestId == test.Id).FirstOrDefault();
			
			testWithData.Test = test;
			testWithData.AuthorName = test.Author?.Username ?? "N/A";
			testWithData.Mark = mark ?? new Mark();
			testWithData.WasTaken = mark != null;
			testWithData.CanTake = test.CanBeTaken(testWithData.WasTaken);

			return Ok(new GetTestWithAuthorNameAndStudentInfoResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = testWithData
			});
		}

		[HttpGet("GetTestsWithSensitiveData")]
		public IActionResult GetDisclosedTests([FromHeader] string? TokenString)
		{
			_logger.LogInformation(nameof(GetDisclosedTests) + " was called.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			if (string.IsNullOrEmpty(tokenString))
				return BadRequest();

			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User user)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			if (user.Type < UserTypes.Teacher)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));
			
			List<Test> tests = _xmtSchoolDbContext.GetAllTests(true);
			List<TestWithAuthorName> testsWithAuthorName = new List<TestWithAuthorName>();

			foreach (Test test in tests)
			{
				string authorUsername = test.Author?.Username ?? "N/A";

				testsWithAuthorName.Add(new TestWithAuthorName(test, authorUsername));
			}

			return Ok(new GetTestsWithSensitiveDataResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = new GetTestsWithSensitiveDataResultData()
				{
					Tests = testsWithAuthorName
				}
			});
		}

		[HttpGet("GetTestWithSensitiveData")]
		public IActionResult GetDisclosedTest([FromHeader] int TestId, [FromHeader] string? TokenString)
		{
			_logger.LogInformation(nameof(GetDisclosedTest) + " was called.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			if (string.IsNullOrEmpty(tokenString))
				return BadRequest();

			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User user)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			if (user.Type == UserTypes.Guest)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			if (_xmtSchoolDbContext.GetTestByTestId(TestId, true) is not Test test)
				return Problem(Utils.GetProblem(ProblemTypes.TestNotFound));

			if (_xmtSchoolDbContext.GetMarksForUser(user.Id) is not List<Mark> marks)
				return Problem(Utils.GetProblem(ProblemTypes.StudentByTokenNotFound));

			if (marks.Where(m => m.TestId == TestId).FirstOrDefault() is not Mark mark)
				return Problem(Utils.GetProblem(ProblemTypes.StudentHasNotTakenThisTest));

			return Ok(new GetTestWithSensitiveDataResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = new GetTestWithSensitiveDataResultData()
				{
					Test = test
				}
			});
		}

		[HttpGet("GetTestsWithSensitiveInfoAndStudentInfo")]
		public IActionResult GetTestsWithSensitiveInfoAndStudentInfo([FromHeader] string? TokenString)
		{
			_logger.LogInformation(nameof(GetTestsWithSensitiveInfoAndStudentInfo) + " was called.");

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			if (string.IsNullOrEmpty(tokenString))
				return BadRequest();

			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User user)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			if (user.Type < UserTypes.Teacher)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			List<Test> tests = _xmtSchoolDbContext.GetAllTests(true);
			List<TestWithAuthorNameAndStudentInfo> testsWithInfo = new List<TestWithAuthorNameAndStudentInfo>();

			foreach (Test test in tests)
			{
				string authorUsername = test.Author?.Username ?? "N/A";
				Mark? mark = _xmtSchoolDbContext.GetMarksForUser(user.Id).Where(m => m.TestId == test.Id).FirstOrDefault();
				bool wasTaken = mark != null;
				bool canTake = test.CanBeTaken(wasTaken);

				testsWithInfo.Add(new TestWithAuthorNameAndStudentInfo(test, authorUsername, wasTaken, canTake, mark ?? new Mark()));
			}

			return Ok(new GetDisclosedTestsWithStudentInfo
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = testsWithInfo
			});
		}

		[HttpPost("AddTest")]
		public IActionResult AddTest([FromBody] AddTestBody Body)
		{
			_logger.LogInformation(nameof(AddTest) + " was called.");

			if (Body == null || Body.Test == null)
				return BadRequest();

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? Body.TokenString ?? string.Empty;

			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User requestingUser)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			if (requestingUser.Type < UserTypes.Teacher)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			try
			{
				Body.Test.SanityCheck();
			}
			catch (Exception ex)
			{
				// Return exception message on why the test is invalid.

				return Problem(string.Format(Utils.GetProblem(ProblemTypes.InvalidTestUploaded), ex.Message));
			}

			// Resetting all IDs.
			Body.Test.Id = 0;

			foreach (Question q in Body.Test.Questions)
			{
				q.Id = 0;

				foreach (Answer a in q.Answers)
				{
					a.Id = 0;
				}
			}

			Body.Test.AuthorUserId = requestingUser.Id; // Overwrite author with the sender author ID.
			Body.Test.CreationDate = DateTime.UtcNow; // Make sure creation time is (UTC) now.

			_xmtSchoolDbContext.AddTest(Body.Test);

			return Ok(new AddTestResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!")
			});
		}

		[HttpDelete("RemoveTest")]
		public IActionResult RemoveTest([FromHeader] string? TokenString, [FromHeader]  int TestId)
		{
			_logger.LogInformation(nameof(RemoveTest) + " was called.");

			if (TestId <= 0)
				return BadRequest(Utils.GetProblem(ProblemTypes.TestNotFound));

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? TokenString ?? string.Empty;

			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User user)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			if (user.Type < UserTypes.Teacher)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			if (_xmtSchoolDbContext.Tests.Find(TestId) is not Test test)
				return Problem(Utils.GetProblem(ProblemTypes.TestNotFound));

			_xmtSchoolDbContext.Tests.Remove(test);
			_xmtSchoolDbContext.SaveChanges();

			return Ok(new RemoveTestResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!")
			});
		}

		[HttpPut("UpdateTest")]
		public IActionResult UpdateTest([FromBody] UpdateTestBody Body)
		{
			_logger.LogInformation(nameof(UpdateTest) + " was called.");

			if (Body == null || Body.Test == null)
				return BadRequest();

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? Body.TokenString ?? string.Empty;

			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User user)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			if (user.Type < UserTypes.Teacher)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotAuthorised));

			if (_xmtSchoolDbContext.Tests.Find(Body.Test.Id) is not Test testInDb)
				return Problem(Utils.GetProblem(ProblemTypes.TestNotFound));

			if (Body.Test.Id != testInDb.Id)
				return Problem(Utils.GetProblem(ProblemTypes.TestNotFound));

			try
			{
				Body.Test.SanityCheck();
			}
			catch (Exception ex)
			{
				return Problem(string.Format(Utils.GetProblem(ProblemTypes.InvalidTestUploaded), ex.Message));
			}

			// We must load the respective questions and answers before
			// we can modify the entity in its entirety.
			_xmtSchoolDbContext.Entry(testInDb)
				.Collection(t => t.Questions) // Include the Questions related to the Test
				.Query()
				.Include(q => q.Answers) // Include the Answers related to each Question
				.Load();

			// Update test in DB with test received
			testInDb.Name = Body.Test.Name;
			testInDb.StartDate = Body.Test.StartDate;
			testInDb.EndDate = Body.Test.EndDate;
			testInDb.RandomiseQuestionsOrder = Body.Test.RandomiseQuestionsOrder;
			testInDb.RandomiseAnswersOrder = Body.Test.RandomiseAnswersOrder;
			testInDb.Questions = Body.Test.Questions;

			// Save changes to persist modifications
			_xmtSchoolDbContext.SaveChanges();

			return Ok(new UpdateTestResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!")
			});
		}

		[HttpPost("SubmitTest")]
		public IActionResult SubmitTest([FromBody] SubmitTestBody Body)
		{
			_logger.LogInformation(nameof(SubmitTest) + " was called.");

			if (Body == null || Body.SubmittableTest == null)
				return BadRequest();

			// Website sends token as a cookie, app sends it in the body.
			string tokenString = Request.Cookies["USER_TOKEN"] ?? Body.TokenString ?? string.Empty;

			if (_xmtSchoolDbContext.GetUserByTokenString(tokenString) is not User user)
				return Problem(Utils.GetProblem(ProblemTypes.UserByTokenNotFound));

			_xmtSchoolDbContext.MarkTokenUsed(tokenString);

			if (user.Type != UserTypes.Student && user.Type != UserTypes.Administrator)
				return Problem(Utils.GetProblem(ProblemTypes.StudentByTokenNotFound));

			List<Mark>? marks = _xmtSchoolDbContext.GetMarksForUser(user.Id);
			bool wasTaken = marks != null && marks.Where(m => m.TestId == Body.SubmittableTest.TestId).FirstOrDefault() != null;

			if (wasTaken)
				return Problem(Utils.GetProblem(ProblemTypes.StudentAlreadyTookTest));

			if (_xmtSchoolDbContext.GetTestByTestId(Body.SubmittableTest.TestId, true) is not Test test)
				return Problem(Utils.GetProblem(ProblemTypes.TestNotFound));

			if (!test.CanBeTaken(wasTaken))
				return Problem(Utils.GetProblem(ProblemTypes.TestNotStarted));

			decimal totalQuestions = test.Questions.Count;

			if (totalQuestions != Body.SubmittableTest.Answers.Length)
				return Problem(Utils.GetProblem(ProblemTypes.InvalidTestSubmitted));

			decimal questionsPassed = 0;

			foreach (Question questionFromDb in test.Questions)
			{
				int questionId = questionFromDb.Id;
				int selectedAnswerId = Body.SubmittableTest.Answers.Where(a => a.QuestionId == questionId).FirstOrDefault()?.AnswerId ?? -1;

				if (selectedAnswerId == -1)
					return Problem(Utils.GetProblem(ProblemTypes.InvalidTestSubmitted));

				if (questionFromDb.Answers.Where(a => a.Id == selectedAnswerId && a.IsValidAnswer).Any())
					questionsPassed++;
			}

			decimal points = questionsPassed / totalQuestions * 100;

			Mark mark = new Mark(0, user.Id, test.Id, points, Body.SubmittableTest.Answers.ToList());

			_xmtSchoolDbContext.Marks.Add(mark);
			_xmtSchoolDbContext.SaveChanges();

			return Ok(new SubmitTestResult
			{
				Message = new ServerMessage(ServerMessageTypes.Success, "Success!"),
				Data = new SubmitTestResultData()
				{
					Mark = mark
				}
			});
		}
	}
}
