using System.ComponentModel.DataAnnotations;

namespace XmtSchoolTypes.Tests
{
	public class SubmittableTest
	{
		[Required]
		public int TestId { get; }

		[Required]
		public SelectedAnswer[] Answers { get; }

		public SubmittableTest(int TestId, SelectedAnswer[] Answers)
		{
			this.TestId = TestId;
			this.Answers = Answers;
		}
	}
}
