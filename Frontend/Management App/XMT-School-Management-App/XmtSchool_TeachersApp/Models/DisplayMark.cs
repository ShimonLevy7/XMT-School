using XmtSchoolTypes.Tests;
using XmtSchoolTypes.Users;

namespace XmtSchool_TeachersApp.Models
{
    public record DisplayMark(int Id, decimal Points, Test Test, User Student);
}
