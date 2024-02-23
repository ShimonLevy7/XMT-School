using XmtSchoolTypes.Users;

namespace XmtSchool_TeachersApp.Enums
{
    internal class UserModel
    {
        public int Id { get; set; }
        public UserTypes Type { get; set; } = UserTypes.Guest;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
