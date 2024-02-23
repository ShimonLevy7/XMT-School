using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace XmtSchoolTypes.Tests
{
	public class SelectedAnswer
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[ForeignKey(nameof(QuestionId))]
		public int QuestionId { get; set; }

		[Required]
		[ForeignKey(nameof(AnswerId))]
		public int AnswerId { get; set; }

		// Foreign keys

		[JsonIgnore]
		public Question? Question { get; set; } // Foreign key between selected answer and question.
		[JsonIgnore]
		public Answer? Answer { get; set; } // Foreign key between selected answer and answer.

		public SelectedAnswer() { }
	}
}
