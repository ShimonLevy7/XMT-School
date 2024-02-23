using System.Windows.Media;
using XmtSchoolTypes.Users;

namespace XmtSchool_TeachersApp.Cache
{
	internal record CurrentUser
	{
		internal static int Id { get; set; } = 0;
		internal static string Username { get; set; } = "N/A";
		internal static string? AvatarUrl { get; set; } = null;
		internal static ImageSource? AvatarImage { get; set; } = null;
		internal static string Email { get; set; } = "N/A";
		internal static UserTypes Type { get; set; } = UserTypes.Guest;
	}
}
