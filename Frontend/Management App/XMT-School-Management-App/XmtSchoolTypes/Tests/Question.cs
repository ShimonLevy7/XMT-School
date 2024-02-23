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

        public Question(int Id, int TestId, string QuestionText)
        {
            this.Id = Id;
            this.TestId = TestId;
            this.QuestionText = QuestionText;
        }
        
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

			foreach (Answer a in Answers)
			{
				try
				{
					a.SanityCheck();
				}
				catch (Exception ex)
				{
					throw new Exception($"Answer #{a.Id} (\"{a.AnswerText}\") is invalid: {ex.Message}");
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
	}
}
