using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using XmtSchoolTypes.Users;

namespace XmtSchoolTypes.Tests
{
	public class Mark
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[ForeignKey(nameof(UserId))]
		public int UserId { get; set; }

		[Required]
		[ForeignKey(nameof(TestId))]
		public int TestId { get; set; }

		[Required]
		public decimal Points { get; set; }

		[Required]
		public List<SelectedAnswer> SelectedAnswers { get; set; } = new List<SelectedAnswer>();

		// Foreign keys

		[JsonIgnore]
		public User? User { get; set; } // Foreign key between mark and student.

		[JsonIgnore]
		public Test? Test { get; set; } // Foreign key between mark and test.

		public Mark()
		{
			// Parameterless constructor
		}

		public Mark(int Id, int UserId, int TestId, decimal Points, List<SelectedAnswer> SelectedAnswers)
		{
			this.Id = Id;
			this.UserId = UserId;
			this.TestId = TestId;
			this.Points = Math.Min(Points, 100); // Sanity verification.
			this.SelectedAnswers = SelectedAnswers;
		}
	}
}
