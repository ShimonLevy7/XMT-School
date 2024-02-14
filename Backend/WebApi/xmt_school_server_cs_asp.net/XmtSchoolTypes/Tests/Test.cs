using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using XmtSchoolTypes.Users;

using XmtSchoolUtils;

namespace XmtSchoolTypes.Tests
{
	public class Test
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[StringLength(128, ErrorMessage = "The {0} must not exceed {1} characters.")]
		public string Name { get; set; } = "N/A";

		[Required]
		public DateTime CreationDate { get; set; }

		[Required]
		[ForeignKey(nameof(AuthorUserId))]
		public int AuthorUserId { get; set; }

		[Required]
		public DateTime StartDate { get; set; }

		[Required]
		public DateTime EndDate { get; set; }

		[Required]
		public bool RandomiseQuestionsOrder { get; set; }

		[Required]
		public bool RandomiseAnswersOrder { get; set; }

		[Required]
		public List<Question> Questions { get; set; } = new List<Question>();

		// Foreign keys

		[JsonIgnore]
		public User? Author { get; set; } // Foreign key between test and author.

		public Test()
		{
			// Parameterless constructor
		}

		public Test(int Id, string Name, DateTime CreationDate, int AuthorUserId,
			DateTime StartDate, DateTime EndDate, bool RandomiseQuestionsOrder, bool RandomiseAnswersOrder, List<Question> Questions)
		{
			this.Id = Id;
			this.Name = Name;
			this.CreationDate = CreationDate;
			this.AuthorUserId = AuthorUserId;
			this.StartDate = StartDate;
			this.EndDate = EndDate;
			this.RandomiseQuestionsOrder = RandomiseQuestionsOrder;
			this.RandomiseAnswersOrder = RandomiseAnswersOrder;
			this.Questions = Questions;
		}

		public Test(int Id, string Name, DateTime CreationDate, int AuthorUserId,
			DateTime StartDate, DateTime EndDate, bool RandomiseQuestionsOrder, bool RandomiseAnswersOrder)
		{
			this.Id = Id;
			this.Name = Name;
			this.CreationDate = CreationDate;
			this.AuthorUserId = AuthorUserId;
			this.StartDate = StartDate;
			this.EndDate = EndDate;
			this.RandomiseQuestionsOrder = RandomiseQuestionsOrder;
			this.RandomiseAnswersOrder = RandomiseAnswersOrder;
		}

		public Test GetUndisclosedTest(bool ShuffleIfRequired = true)
		{
			if (IsUpcoming()) // Questions are not revealed before the test is ongoing.
				return new Test(Id, Name, CreationDate, AuthorUserId, StartDate, EndDate, false, false);

			// Shuffle questions.
			if (ShuffleIfRequired && RandomiseQuestionsOrder)
				Questions.Shuffle();

			for (short i = 0; i < Questions.Count; i++)
			{
				Question question = Questions[i];

				// Shuffle answers.
				if (ShuffleIfRequired && RandomiseAnswersOrder)
				{
					question.Answers.Shuffle();

					foreach (Answer answer in question.Answers)
					{
						// Hide the real answer (set all to `false`).
						answer.IsValidAnswer = false;
					}
				}
			}

			// Hide that the questions and/or answers are randomised.
			return new Test(Id, Name, CreationDate, AuthorUserId, StartDate, EndDate, false, false, Questions);
		}

		public void SanityCheck()
		{
			if (string.IsNullOrEmpty(Name) || Name == "N/A")
				throw new Exception("Test name is invalid.");

			if (AuthorUserId <= 0)
				throw new Exception("Author user ID is invalid.");

			if (EndDate <= StartDate)
				throw new Exception("End date cannot be on or earlier than the start date.");

			if (Questions.Count < 2)
				throw new Exception("At least 2 questions are required.");

			foreach (Question q in Questions)
			{
				try
				{
					q.SanityCheck();
				}
				catch (Exception ex)
				{
					throw new Exception($"Question \"{q.QuestionText}\" is invalid: {ex.Message}");
				}
			}
		}

		public bool IsValid()
		{
			try
			{
				SanityCheck();

				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool IsUpcoming()
		{

			return StartDate > DateTime.UtcNow;
		}

		public bool IsExpired()
		{

			return EndDate < DateTime.UtcNow;
		}

		public bool CanBeTaken(bool WasTaken)
		{
			if (!WasTaken // Not taken.
				&& !IsExpired() && !IsUpcoming()) // Within time frame.
			{
				return true;
			}

			return false;
		}
	}

	/// <summary>
	/// This class holds a test and various related student info.
	/// </summary>
	public class TestWithAuthorNameAndStudentInfo
	{
		public Test Test { get; set; } = new Test();
		public string AuthorName { get; set; } = string.Empty;
		public bool WasTaken { get; set; } = false;
		public bool CanTake { get; set; } = false;
		public Mark Mark { get; set; } = new Mark();

		public TestWithAuthorNameAndStudentInfo()
		{

		}

		public TestWithAuthorNameAndStudentInfo(Test Test, string AuthorName, bool WasTaken, bool CanTake, Mark Mark)
		{
			this.Test = Test;
			this.AuthorName = AuthorName;
			this.WasTaken = WasTaken;
			this.CanTake = CanTake;
			this.Mark = Mark;
		}
	}

	/// <summary>
	/// This class holds a test and its author name.
	/// </summary>
	public class TestWithAuthorName
	{
		public Test Test { get; set; } = new Test();
		public string AuthorName { get; set; } = string.Empty;

		public TestWithAuthorName()
		{

		}

		public TestWithAuthorName(Test Test, string AuthorName)
		{
			this.Test = Test;
			this.AuthorName = AuthorName;
		}
	}
}
