using XmtSchoolTypes.Tests;

namespace XmtSchoolWebApi.Types.BodyTypes
{
	public class TestsBodyTypes
	{
		public record AddTestBody
		{
			public string? TokenString { get; set; } = string.Empty;
			public Test? Test { get; set; } = null;
		}

		public record UpdateTestBody
		{
			public string? TokenString { get; set; } = string.Empty;
			public Test? Test { get; set; } = null;
		}

		public record SubmitTestBody
		{
			public string? TokenString { get; set; } = string.Empty;
			public SubmittableTest? SubmittableTest { get; set; } = null;
		}
	}
}
