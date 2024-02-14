using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace XmtSchoolTypes.Tests
{
	public class Question
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[ForeignKey(nameof(TestId))]
		public int TestId { get; set; }

		[Required]
		[StringLength(1024, ErrorMessage = "The {0} must not exceed {1} characters.")]
		public string QuestionText { get; set; } = "N/A";

		[Required]
		public List<Answer> Answers { get; set; } = new List<Answer>();

		// Foreign keys

		[JsonIgnore]
		public Test? Test { get; set; } // Foreign key between test and questions.

		public Question() { }

		public Question(int Id, int TestId, string QuestionText, List<Answer> Answers)
		{
			this.Id = Id;
			this.TestId = TestId;
			this.QuestionText = QuestionText;
			this.Answers = Answers;
		}

		public Question(string QuestionText, List<Answer> Answers)
		{
			this.QuestionText = QuestionText;
			this.Answers = Answers;
		}

		public void SanityCheck()
		{
			if (string.IsNullOrEmpty(QuestionText) || QuestionText == "N/A")
				throw new Exception("Invalid question text.");

			if (Answers.Count < 2)
				throw new Exception("At least 2 answers are required.");

			bool hasRightAnswer = false;
			bool hasWrongAnswer = false;

			foreach (Answer a in Answers)
			{
				try
				{
					// Check if this answer is marked as wrong or right 
					// and keep track of the result for later.
					hasRightAnswer = a.IsValidAnswer || hasRightAnswer;
					hasWrongAnswer = !a.IsValidAnswer || hasWrongAnswer;

					a.SanityCheck();
				}
				catch (Exception ex)
				{
					throw new Exception($"Answer \"{a.AnswerText}\" is invalid: {ex.Message}");
				}
			}

			if (!hasRightAnswer)
				throw new Exception($"At least one right answer is required.");

			if (!hasWrongAnswer)
				throw new Exception($"At least one wrong answer is required.");
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
	}
}
