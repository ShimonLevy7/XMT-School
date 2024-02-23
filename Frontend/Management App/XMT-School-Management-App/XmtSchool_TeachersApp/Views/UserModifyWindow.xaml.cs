using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XmtSchool_TeachersApp.Api;
using XmtSchool_TeachersApp.Cache;
using XmtSchool_TeachersApp.Models;
using XmtSchool_TeachersApp.Utils;
using XmtSchoolTypes.Users;
using XmtSchoolUtils;
using XmtSchoolWebApi.Types;
using static XmtSchoolWebApi.Types.ResultTypes.UsersResultTypes;

namespace XmtSchool.TeachersAppViews
{
    /// <summary>
    /// Interaction logic for UserModifyWindow.xaml
    /// </summary>
    public partial class UserModifyWindow : Window
    {
        private readonly UserAction actionType;
        private readonly MainWindow mainWindow;
        private readonly User? userToUpdate;

        public UserModifyWindow(MainWindow mainWindow, User? userToUpdate = null)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.userToUpdate = userToUpdate;

            actionType = userToUpdate == null
                ? UserAction.Create
                : UserAction.Update;

            DataContext = new UserModifyViewModel(userToUpdate);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Username_TextBox.Focus();

            if (actionType == UserAction.Create)
            {
                Title = "XMT School - Management App - Create a new user";
                TitleTextBox.Text = $"Create a new user";

                UserType_ComboBox.SelectedIndex = 0;

                Confirm_Button.Content = "Create";

                return;
            }

            if (userToUpdate == null)
                return;

            Title = $"XMT School - Management App - Update user {userToUpdate.Username}";
            TitleTextBox.Text = $"Update {userToUpdate.Username}";
            UserType_ComboBox.SelectedItem = Enum.GetName(userToUpdate.Type);
            Username_TextBox.Text = userToUpdate.Username;
            Email_TextBox.Text = userToUpdate.Email;
            AvatarString_TextBox.Text = userToUpdate.AvatarUrl;

            Confirm_Button.Content = "Update";
        }

        private void PictureString_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Clear loaded image.
            Avatar_Image.ImageSource = null; 

            // Load new image.
            new Thread(async () =>
            {
                Thread.CurrentThread.IsBackground = true;

                await Dispatcher.InvokeAsync(async () =>
                {
                    string url = ((TextBox)sender).Text;

                    ImageSource? img = string.IsNullOrWhiteSpace(url)
                        ? await Utils.GetImageSourceFromUrlAsync($"https://ui-avatars.com/api/?name={CurrentUser.Username.Replace(' ', '+')}?size=4096")
                        : await Utils.GetImageSourceFromUrlAsync(url);

					Avatar_Image.ImageSource = img;
                });

            }).Start();
        }

        private async void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            // Introduce a timeout using Task.Delay
            Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(10));

            switch (actionType)
            {
                case UserAction.Create:
                    User userToAdd = new User(0, (UserTypes)Enum.Parse(typeof(UserTypes), UserType_ComboBox.SelectedValue.ToString() ?? string.Empty), Username_TextBox.Text, Password_TextBox.Password, Email_TextBox.Text, AvatarString_TextBox.Text);

                    // Use Task.Run to run the AddUser method on a background thread
                    Task<AddUserResult?> addUserTask = Task.Run(() => Api.AddUserAsync(userToAdd));

                    Confirm_Button.IsEnabled = false;
                    Cancel_Button.IsEnabled = false;

                    // Wait for either addUserTask or timeoutTask to complete
                    await Task.WhenAny(addUserTask, timeoutTask);

                    Confirm_Button.IsEnabled = true;
                    Cancel_Button.IsEnabled = true;

                    // Check if the API call completed successfully
                    if (!addUserTask.IsCompleted)
                    {
                        // Handle the case where the timeoutTask completed first
                        MessageBox.Show("API request timed out.", "Timeout Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                        return;
                    }
                    
                    // Get the result from the completed task
                    AddUserResult? addUserResult = addUserTask.Result;

                    if (addUserResult == null)
                        return; // Handled by AddUser method.

                    if (addUserResult.Message.MessageType != BaseApi.ServerMessageTypes.Success)
                    {
                        MessageBox.Show(addUserResult.Message.Message, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        return;
                    }

                    MessageBox.Show("User added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    await mainWindow.InitUsersListAsync(true); // Refresh user list on main window.

                    Close();

                    break;

                case UserAction.Update:
                    if (userToUpdate == null)
                        throw new Exception("Attempt to update invalid user.");

                    User userToUpdateCopy = new User(userToUpdate.Id, (UserTypes)Enum.Parse(typeof(UserTypes), UserType_ComboBox.SelectedValue.ToString() ?? string.Empty), Username_TextBox.Text, Password_TextBox.Password, Email_TextBox.Text, AvatarString_TextBox.Text);

                    // Use Task.Run to run the AddUser method on a background thread
                    Task<UpdateUserResult?> updateUserTask = Task.Run(() => Api.UpdateUserAsync(userToUpdateCopy));

                    // Introduce a timeout using Task.Delay

                    Confirm_Button.IsEnabled = false;
                    Cancel_Button.IsEnabled = false;

                    // Wait for either addUserTask or timeoutTask to complete
                    await Task.WhenAny(updateUserTask, timeoutTask);

                    Confirm_Button.IsEnabled = true;
                    Cancel_Button.IsEnabled = true;

                    // Check if the API call completed successfully
                    if (!updateUserTask.IsCompleted)
                    {
                        // Handle the case where the timeoutTask completed first
                        MessageBox.Show("API request timed out.", "Timeout Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                        return;
                    }

                    // Get the result from the completed task
                    UpdateUserResult? updateUserResult = updateUserTask.Result;

                    if (updateUserResult == null)
                        return; // Handled by UpdateUser method.

                    if (updateUserResult.Message.MessageType != BaseApi.ServerMessageTypes.Success)
                    {
                        MessageBox.Show(updateUserResult.Message.Message, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        return;
                    }

                    MessageBox.Show("User updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    await mainWindow.InitUsersListAsync(true); // Refresh user list on main window.

                    Close();

                    break;
            }
        }
    }
}
