using static XmtSchoolWebApi.Types.BaseApi;
using XmtSchoolTypes.Tests;

namespace XmtSchoolWebApi.Types.ResultTypes
{
	public class MarksResultTypes
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////
		// GET
		////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Get Student Marks
		/// </summary>
		public class GetStudentMarksResult : IServerResponse
		{
			public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
			public List<Mark> Data { get; set; } = new List<Mark>();
		}

        /// <summary>
        /// Get all marks in the school
        /// </summary>
        public class GetAllMarksResult : IServerResponse
        {
            public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
            public List<Mark> Data { get; set; } = new List<Mark>();
        }

        /// <summary>
        /// Deletes a mark
        /// </summary>
        public class RemoveMarkResult : IServerResponse
        {
            public ServerMessage Message { get; set; } = new ServerMessage(ServerMessageTypes.Info, "No message specified.");
        }
    }
}
