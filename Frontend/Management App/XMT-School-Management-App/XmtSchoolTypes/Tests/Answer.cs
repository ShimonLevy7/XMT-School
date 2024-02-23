using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace XmtSchoolTypes.Tests
{
	public class Answer
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[ForeignKey(nameof(QuestionId))]
		public int QuestionId { get; set; }

		[Required]
		[StringLength(512, ErrorMessage = "The {0} must not exceed {1} characters.")]
		public string AnswerText { get; set; } = "N/A";

		[Required]
		public bool IsValidAnswer { get; set; }

		// Foreign keys

		[JsonIgnore]
		public Question? Question { get; set; } = null; // Foreign key between question and answer.

		public Answer() { }

		public Answer(string AnswerText, bool IsValidAnswer)
		{
			this.AnswerText = AnswerText;
			this.IsValidAnswer = IsValidAnswer;
		}

		public Answer(short Id, int QuestionId, string AnswerText, bool IsValidAnswer)
		{
			this.Id = Id;
			this.QuestionId = QuestionId;
			this.AnswerText = AnswerText;
			this.IsValidAnswer = IsValidAnswer;
		}

		public void SanityCheck()
		{
			if (string.IsNullOrEmpty(AnswerText) || AnswerText == "N/A")
				throw new Exception("Invalid answer text.");
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
