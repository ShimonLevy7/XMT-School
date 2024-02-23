using XmtSchoolTypes.Tests;

namespace XmtSchoolWebApi.Types.BodyTypes
{
	public class TestsBodyTypes
	{
		public record AddTestBody
		{
			public string TokenString { get; set; } = string.Empty;
			public Test? Test { get; set; } = null;
			
			public AddTestBody (string TokenString, Test? Test)
			{
				this.TokenString = TokenString;
				this.Test = Test;
			}
		}

		public record RemoveTestBody
		{
			public string TokenString { get; set; } = string.Empty;
			public int TestId { get; set; } = 0;
		}

        public record UpdateTestBody
        {
            public string TokenString { get; set; } = string.Empty;
            public Test? Test { get; set; } = null;

            public UpdateTestBody(string tokenString, Test? test)
            {
                TokenString = tokenString;
                Test = test;
            }
        }

        public record SubmitTestBody
		{
			public string TokenString { get; set; } = string.Empty;
			public SubmittableTest? SubmittableTest { get; set; } = null;
		}
	}
}
