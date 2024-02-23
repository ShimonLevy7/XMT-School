using System;
using System.Linq;
using XmtSchoolTypes.Users;

namespace XmtSchool_TeachersApp.Models
{
    public class UserModifyViewModel
    {
        // The list of user types, a static value.
        public string[] UserTypesArray { get; private set; }

        // Property to hold the selected user, if any.
        public User? SelectedUser { get; private set; }

        public UserModifyViewModel(User? selectedUser = null)
        {
            string[] userTypes = Enum.GetNames(typeof(UserTypes));

            userTypes = userTypes.Skip(1).ToArray();

            UserTypesArray = userTypes;
            SelectedUser = selectedUser;
        }
    }
}
