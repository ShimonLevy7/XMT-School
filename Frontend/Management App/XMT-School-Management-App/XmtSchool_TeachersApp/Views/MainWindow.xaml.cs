using System;
using System.Collections.Generic;
using System.Windows;
using XmtSchool_TeachersApp.Api;
using XmtSchool_TeachersApp.Cache;
using XmtSchoolTypes.Tests;
using XmtSchoolTypes.Users;
using System.Threading.Tasks;
using System.Windows.Controls;
using XmtSchool_TeachersApp.Models;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Input;
using XmtSchoolWebApi.Types;
using System.Windows.Data;
using XmtSchool_TeachersApp.Views;
using XmtSchool_TeachersApp.Utils;
using System.Diagnostics;
using System.Linq;
using System.Collections.ObjectModel;
using static XmtSchoolWebApi.Types.ResultTypes.TestsResultTypes;
using static XmtSchoolWebApi.Types.ResultTypes.UsersResultTypes;
using static XmtSchoolWebApi.Types.ResultTypes.MarksResultTypes;

namespace XmtSchool.TeachersAppViews
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            _cache = new ApiCache();

            InitializeComponent();
        }

        private readonly ApiCache _cache; // Caching the data so it doesn't take it from the API more than once. unless refreshed.

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Set window title with current user's information.
            Title = "XMT School - Management App - Logged in as " + CurrentUser.Username + " (" + CurrentUser.Type + ")";

            // Display current user's avatar in the window.
            Avatar_Image.Source = CurrentUser.AvatarImage;
            ProfileAvatarImage.ImageSource = CurrentUser.AvatarImage;

            // Display current user's profile information in the window.
            ProfileUsername.Content = CurrentUser.Username;
            ProfileEmail.Content = CurrentUser.Email;
            ProfileAccountType.Content = CurrentUser.Type;

            // Load and initialize the user management list.
            // If the loading fails, hide the Users Management Tab.
            if (!await InitUsersListAsync(true))
                UsersMgtTab.Visibility = Visibility.Collapsed;

            // Load and initialize the test management list.
            // If the loading fails, hide the Test Management Tab.
            if (!await InitTestsListAsync(true))
                TestsMgtTab.Visibility = Visibility.Collapsed;

            // Load and initialize the marks management list.
            // If the loading fails, hide the Test Management Tab.
            if (!await InitMarksListAsync(true))
                StudentsMks.Visibility = Visibility.Collapsed;
        } // Event handler for when the window is loaded.

        // Initializes the list of users asynchronously.
        internal async Task<bool> InitUsersListAsync(bool fetchNewData)
        {
            // Check if the current user is authorized (must be an administrator).
            if (CurrentUser.Type < UserTypes.Administrator)
                return false;  // Return false if the user is not authorized.

            // Check if new data needs to be fetched.
            if (fetchNewData)
            {
                // Fetch and update users data asynchronously.
                await _cache.UpdateUsersAsync();
            }

            // Retrieve the updated list of users from the cache.
            List<User> users = _cache.Users;

            // Clear the UserList UI element before adding new items.
            UserList.ItemsSource = null;
            UserList.Items.Clear();  // Clears any existing items in the list (this could be considered redundant after setting ItemsSource to null).

            // Assign the users list as the data source for the UserList UI element.
            UserList.ItemsSource = users;

            // Fix the width of the list columns
            if (UserList.View is GridView gv)
            {
                foreach (GridViewColumn? c in gv.Columns)
                {
                    if (double.IsNaN(c.Width))
                    {
                        c.Width = ActualWidth;
                    }

                    c.Width = double.NaN;
                }
			}

			SetFilterOptions<User>(UserList, UserFilterOptions_ComboBox, _userFilterOptions, FilterUsersFunc);

			return true;  // Return true indicating the list has been successfully initialized.
        }

        // Initializes the list of tests asynchronously.
        internal async Task<bool> InitTestsListAsync(bool fetchNewData)
        {
            // Check if the current user is authorized (must be at least a teacher).
            if (CurrentUser.Type < UserTypes.Teacher)
                return false;  // Return false if the user is not authorized.

            // Check if new data needs to be fetched.
            if (fetchNewData)
            {
                // Fetch and update tests data asynchronously.
                await _cache.UpdateTestsAsync();
            }

            // Retrieve the updated list of tests from the cache.
            List<TestWithAuthorName>? tests = _cache.Tests;

            // Assign the tests list as the data source for the TestList UI element.
            TestList.ItemsSource = tests;

            // fix the width of the list columns
            if (TestList.View is GridView gv)
            {
                foreach (GridViewColumn? c in gv.Columns)
                {
                    if (double.IsNaN(c.Width))
                    {
                        c.Width = ActualWidth;
                    }

                    c.Width = double.NaN;
                }
            }

			SetFilterOptions<TestWithAuthorName>(TestList, TestFilterOptions_ComboBox, _testFilterOptions, FilterTestsFunc);

			return true;  // Return true indicating the list has been successfully initialized.
        }

		// Initializes the list of marks asynchronously.
		internal async Task<bool> InitMarksListAsync(bool fetchNewData)
        {
            // Check if the current user is authorized (must be at least a teacher).
            if (CurrentUser.Type < UserTypes.Teacher)
                return false;  // Return false if the user is not authorized.

            // Check if new data needs to be fetched.
            if (fetchNewData)
            {
                // Fetch and update tests data asynchronously.
                await _cache.UpdateMarksAsync();
            }

            // Retrieve the updated list of tests from the cache.
            List<Mark>? marks = _cache.Marks;

            List<DisplayMark> displayMarks = new List<DisplayMark>();

            foreach (Mark mark in marks)
            {
                TestWithAuthorName? test = _cache.Tests.Where(t => t.Test.Id == mark.TestId).FirstOrDefault();

                if (test is null)
                    continue;

                User? student = _cache.Users.Where(u => u.Id == mark.UserId).FirstOrDefault();

                if (student is null)
                    continue;

                displayMarks.Add(new DisplayMark(mark.Id, mark.Points, test.Test, student));
            }

            // Assign the tests list as the data source for the TestList UI element.
            MarkList.ItemsSource = displayMarks;

            // fix the width of the list columns
            if (MarkList.View is GridView gv)
            {
                foreach (GridViewColumn? c in gv.Columns)
                {
                    if (double.IsNaN(c.Width))
                    {
                        c.Width = ActualWidth;
                    }

                    c.Width = double.NaN;
                }
			}

			SetFilterOptions<DisplayMark>(MarkList, MarkFilterOptions_ComboBox, _markFilterOptions, FilterMarksFunc);

			return true;
        }

        // Buttons - User

        private async void RefreshUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Disable the window
                IsEnabled = false;

                // Asynchronously reinitialize the users list to fetch new data.
                await InitUsersListAsync(true);
            }
            finally
            {
                // Create a timer to re-enable the refresh button after a 3-second delay.
                new MtTimer(TimeSpan.FromSeconds(3), 1, () => Dispatcher.Invoke(() => IsEnabled = true)).Start();
            }
        }  // Refresh button event handler

        private async void UserAddBtn_Click(object sender, RoutedEventArgs e)
        {
            await AddUserAsync();
        } // Add button event handler

        private void UserUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateUser();
        } // Update/Modify button event handler

        private async void UserRemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            await RemoveUserAsync();
        } // Remove button event handler

        // Buttons - Tests

        private async void RefreshTestsBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsEnabled = false; 

                await InitTestsListAsync(true);
            }
            finally
            {
                // Create a timer to re-enable the refresh button after a 3-second delay.
                new MtTimer(TimeSpan.FromSeconds(3), 1, () => Dispatcher.Invoke(() => IsEnabled = true)).Start();
            }
        } // Event handler for the RefreshTestsBtn click event, which refreshes the tests list.

        private async void TestAddBtn_Click(object sender, RoutedEventArgs e)
        {
            await AddTestAsync();
        } // Stub event handler for the TestAddBtn click event.

        private async void TestUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            await UpdateTestAsync();
        } // Handles the Click event for the 'Update Test' button.

        private async void TestRemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            await RemoveTestAsync();
        } // Stub event handler for the TestRemoveBtn click event.

        // Buttons - Marks

        private async void RefreshMarksBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsEnabled = false;

                await InitMarksListAsync(true);
            }
            finally
            {
                // Create a timer to re-enable the refresh button after a 3-second delay.
                new MtTimer(TimeSpan.FromSeconds(3), 1, () => Dispatcher.Invoke(() => IsEnabled = true)).Start();
            }
        }

        private void MarkViewBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewMark();
        }

        private async void MarkRemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            await RemoveMarkAsync();
        }

        // Buttons - General

        private void Logout(object sender, RoutedEventArgs e)
        {
            // Displays a confirmation dialog to the user with a yes/no option for logout.
            MessageBoxResult logoutConfirmation = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // If the user chooses anything other than 'Yes', exit the function without logging out.
            if (logoutConfirmation != MessageBoxResult.Yes)
                return;

            // Creates a new instance of the LoginWindow.
            LoginWindow loginWindow = new LoginWindow();

            // Shows the Login window to the user.
            loginWindow.Show();

            // Closes the current (main) window, effectively logging the user out.
            Close();
        } // Handles the "Logout" event when the user opts to log out of the application.

        // Context menu - User

        private async void ContextUserAdd_Click(object sender, RoutedEventArgs e)
        {
            // Call the asynchronous method to add a new user.
            await AddUserAsync();
        } // Unselects all users when the right mouse button is pressed down on the UserList.

        private void ContextUserUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Call the method to update the selected user's information.
            UpdateUser();
        } // Handles the Click event for the 'Update User' context menu option.

        private async void ContextUserRemove_Click(object sender, RoutedEventArgs e)
        {
            await RemoveUserAsync();
        } // Handles the click event for the 'User Remove' context menu option.

        // Context menu - Tests

        private async void ContextTestAdd_Click(object sender, RoutedEventArgs e)
        {
            // Call the asynchronous method to add new test.
            await AddTestAsync();
        } // Handles the click event for the 'Add Test' context menu option.

        private async void ContextTestUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Call the method to update the selected test's information
            await UpdateTestAsync();
        } // Handles the click event for the 'Update Test' context menu option.

        private async void ContextTestRemove_Click(object sender, RoutedEventArgs e)
        {
            await RemoveTestAsync();
        } // Handles the Click event for the 'Remove Test' context menu option.

        // Context Menu - Marks //
        private void ContextMarkView_Click(object sender, RoutedEventArgs e)
        {
            ViewMark();
        }

        private async void ContextMarkRemove_Click(object sender, RoutedEventArgs e)
        {
            await RemoveMarkAsync();
        }

        // Methods and logic //

        #region Selection trackers
        /// <summary>
        /// Keeps track of selected User from the list.
        /// </summary>
        /// <param name="sender">example 1</param>
        /// <param name="e">example 2</param>
        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Return if no items were added to the selection.
            if (e.AddedItems.Count <= 0)
                return;

            // Get the currently selected item.
            object? selectedRow = e.AddedItems[0];

            // Check if the selected item is a User and if not, return.
            if (selectedRow is not User user)
                return;

            // Enable or disable the remove user buttons based on whether the selected user is not the current user.
            UserRemoveBtn.IsEnabled = user.Id != CurrentUser.Id;
            ContextUserRemove.IsEnabled = user.Id != CurrentUser.Id;
        }
        #endregion

        // Add, Update, Remove, View tasks

        private async Task AddUserAsync()
        {
            // Create a new instance of the UserModifyWindow intended for user creation.
            UserModifyWindow userModifyWindow = new UserModifyWindow(this);

            // Display the window as a modal dialog, blocking interaction with the parent window.
            userModifyWindow.ShowDialog();

            // Re-initialize the users list to include the newly added user.
            // 'true' indicates that the method should fetch new data.
            await InitUsersListAsync(true);
        } // Asynchronously adds a new user to the system.

        private async Task AddTestAsync()
        {
            // Creates a new instance of the TestModifyWindow intended for test creation.
            TestModifyWindow testModifyWindow = new TestModifyWindow(TestAction.Create, new TestWithAuthorName()
            {
                Test = new Test()
                {
                    Questions = new List<Question>()
                }
            });

            // Display the window as a modal dialog, blocking interaction with the parent window.
            testModifyWindow.ShowDialog();

            // Re-initialize the tests list to include the newly added test.
            // 'true' indicates that the method should fetch new data.
            await InitTestsListAsync(true);
        } // Asynchronously adds a new test to the system.

        private Task UpdateUser()
        {
            // Retrieve the currently selected user from the UserList.
            User selectedUser = (User)UserList.SelectedItem;

            // If no user is selected, display an error message and return a completed Task.
            if (selectedUser == null)
            {
                // Use MessageBoxBuilderModel to construct and display a message box.
                MessageBoxBuilderModel messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = "Must select a user to modify.",
                    Icon = MessageBoxImage.Error,
                    Title = "User Modification",
                    Button = MessageBoxButton.OK
                };

                // Display the constructed error message box.
                messageBuilder.ShowMessage();

                // Return a completed Task to satisfy the Task-returning signature of the method.
                return Task.CompletedTask;
            }

            // Open the UserModifyWindow for editing the selected user's details.
            try
            {
                UserModifyWindow userModifyWindow = new UserModifyWindow(this, selectedUser);
                userModifyWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                // An exception occurred during the operation. Show an error message.
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Return a completed Task to end the method execution.
            return Task.CompletedTask;
        } // Updates an existing user's information.

        private async Task UpdateTestAsync()
        {
            // Retrieve the currently selected test from the TestList.
            TestWithAuthorName selectedTest = (TestWithAuthorName)TestList.SelectedItem;

            // If no test is selected, display an error message and return a completed Task.
            if (selectedTest == null)
            {
                // Use MessageBoxBuilderModel to construct and display a message box.
                MessageBoxBuilderModel messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = "Must select a test to modify.",
                    Icon = MessageBoxImage.Error,
                    Title = "Test Modification",
                    Button = MessageBoxButton.OK
                };

                // Display the constructed error message box.
                messageBuilder.ShowMessage();

                // Return a completed Task to satisfy the Task-returning signature of the method.
                return;
            }

            // Open the UserModifyWindow for editing the selected user's details.
            try
            {
                TestModifyWindow testModifyWindow = new TestModifyWindow(TestAction.Update, selectedTest);

                testModifyWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                // An exception occurred during the operation. Show an error message.
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            await InitTestsListAsync(true); // Refresh user list on main window.

            return;
        }

        private async Task RemoveUserAsync()
        {
            // Attempt to cast the selected item in UserList to a User object.
            User selectedUser = (User)UserList.SelectedItem;
            MessageBoxBuilderModel messageBuilder;

            // If no user is selected, display an error message and return early.
            if (selectedUser == null)
            {
                messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = "Must select a user to delete.",
                    Icon = MessageBoxImage.Error,
                    Title = "User Deletion",
                    Button = MessageBoxButton.OK
                };

                messageBuilder.ShowMessage();
                return;
            }

            // Ask for confirmation before deletion.
            messageBuilder = new MessageBoxBuilderModel()
            {
                Message = $"Are you sure you want to delete the user \"{selectedUser.Username}\"?",
                Icon = MessageBoxImage.Warning,
                Title = "User deletion confirmation",
                Button = MessageBoxButton.YesNo
            };

            // If the user does not confirm deletion, return early.
            if (messageBuilder.ShowMessage() != MessageBoxResult.Yes)
                return;

            // Disable the UI to prevent further action while the deletion is processing.
            IsEnabled = false;

            // Try to remove the user asynchronously and capture the result.
            RemoveUserResult? result = await Api.RemoveUserAsync(selectedUser.Id);

            // Check if we received a result from the API call.
            if (result != null)
            {
                // The message type in the result will indicate whether the operation succeeded.
                if (result.Message.MessageType != BaseApi.ServerMessageTypes.Success)
                {
                    // If not successful, show an error message.
                    messageBuilder = new MessageBoxBuilderModel()
                    {
                        Message = result.Message.Message,
                        Icon = MessageBoxImage.Error,
                        Title = "User Deletion",
                        Button = MessageBoxButton.OK
                    };
                }
                else
                {
                    // If successful, show an information message.
                    messageBuilder = new MessageBoxBuilderModel()
                    {
                        Message = "User deleted successfully.",
                        Icon = MessageBoxImage.Information,
                        Title = "User Deletion",
                        Button = MessageBoxButton.OK
                    };
                }
                // Display the message from the result.
                messageBuilder.ShowMessage();
            }

            // Reinitialize the users list to reflect the changes after deletion.
            await InitUsersListAsync(true);

            // Re-enable the UI after processing is complete.
            IsEnabled = true;
        } // Asynchronously removes the selected user from the system.

        private async Task RemoveTestAsync()
        {
            // Attempt to cast the selected item in TestList to a Test object.
            TestWithAuthorName selectedTest = (TestWithAuthorName)TestList.SelectedItem;
            MessageBoxBuilderModel messageBuilder;

            // If no Test is selected, display an error message and return early.
            if (selectedTest == null)
            {
                messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = "Must select a Test to delete.",
                    Icon = MessageBoxImage.Error,
                    Title = "Test Deletion",
                    Button = MessageBoxButton.OK
                };

                messageBuilder.ShowMessage();
                return;
            }

            // Ask for confirmation before deletion.
            messageBuilder = new MessageBoxBuilderModel()
            {
                Message = $"Are you sure you want to delete the test \"{selectedTest.Test.Name}\"?",
                Icon = MessageBoxImage.Warning,
                Title = "Test deletion confirmation",
                Button = MessageBoxButton.YesNo
            };

            // If the Test does not confirm deletion, return early.
            if (messageBuilder.ShowMessage() != MessageBoxResult.Yes)
                return;

            // Disable the UI to prevent further action while the deletion is processing.
            IsEnabled = false;

            // Try to remove the Test asynchronously and capture the result.
            RemoveTestResult? result = await Api.RemoveTestAsync(selectedTest.Test.Id);

            // Check if we received a result from the API call.
            if (result != null)
            {
                // The message type in the result will indicate whether the operation succeeded.
                if (result.Message.MessageType != BaseApi.ServerMessageTypes.Success)
                {
                    // If not successful, show an error message.
                    messageBuilder = new MessageBoxBuilderModel()
                    {
                        Message = result.Message.Message,
                        Icon = MessageBoxImage.Error,
                        Title = "Test Deletion",
                        Button = MessageBoxButton.OK
                    };
                }
                else
                {
                    // If successful, show an information message.
                    messageBuilder = new MessageBoxBuilderModel()
                    {
                        Message = "Test deleted successfully.",
                        Icon = MessageBoxImage.Information,
                        Title = "Test Deletion",
                        Button = MessageBoxButton.OK
                    };
                }
                // Display the message from the result.
                messageBuilder.ShowMessage();
            }

            // Reinitialize the Tests list to reflect the changes after deletion.
            await InitTestsListAsync(true);

            // Re-enable the UI after processing is complete.
            IsEnabled = true;

        }

        private async Task RemoveMarkAsync()
        {
            // Attempt to cast the selected item in MarkList to a Mark object.
            DisplayMark selectedMark = (DisplayMark)MarkList.SelectedItem;
            MessageBoxBuilderModel messageBuilder;

            // If no Mark is selected, display an error message and return early.
            if (selectedMark == null)
            {
                messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = "Must select a Mark to delete.",
                    Icon = MessageBoxImage.Error,
                    Title = "Mark Deletion",
                    Button = MessageBoxButton.OK
                };

                messageBuilder.ShowMessage();
                return;
            }

            // Ask for confirmation before deletion.
            messageBuilder = new MessageBoxBuilderModel()
            {
                Message = $"Are you sure you want to delete mark \"{selectedMark.Id}\"?",
                Icon = MessageBoxImage.Warning,
                Title = "Mark deletion confirmation",
                Button = MessageBoxButton.YesNo
            };

            // If the Mark does not confirm deletion, return early.
            if (messageBuilder.ShowMessage() != MessageBoxResult.Yes)
                return;

            // Disable the UI to prevent further action while the deletion is processing.
            IsEnabled = false;

            // Try to remove the Mark asynchronously and capture the result.
            RemoveMarkResult? result = await Api.RemoveMarkAsync(selectedMark.Id);

            // Check if we received a result from the API call.
            if (result != null)
            {
                // The message type in the result will indicate whether the operation succeeded.
                if (result.Message.MessageType != BaseApi.ServerMessageTypes.Success)
                {
                    // If not successful, show an error message.
                    messageBuilder = new MessageBoxBuilderModel()
                    {
                        Message = result.Message.Message,
                        Icon = MessageBoxImage.Error,
                        Title = "Mark Deletion",
                        Button = MessageBoxButton.OK
                    };
                }
                else
                {
                    // If successful, show an information message.
                    messageBuilder = new MessageBoxBuilderModel()
                    {
                        Message = "Mark deleted successfully.",
                        Icon = MessageBoxImage.Information,
                        Title = "Mark Deletion",
                        Button = MessageBoxButton.OK
                    };
                }
                // Display the message from the result.
                messageBuilder.ShowMessage();
            }

            // Reinitialize the Marks list to reflect the changes after deletion.
            await InitMarksListAsync(true);

            // Re-enable the UI after processing is complete.
            IsEnabled = true;
        }

        private async void UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the selected user from the UserList.
            User selectedUser = (User)UserList.SelectedItem;

            // Initialize UserModifyWindow with the selected user and show it as a modal dialog.
            UserModifyWindow userModifyWindow = new UserModifyWindow(this, selectedUser);
            userModifyWindow.ShowDialog();

            // Refresh the list of users after the modification.
            await InitUsersListAsync(true);
        } // Event handler for clicking the button to update a user's details.

        private async void RemoveUser_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the selected user from the UserList.
            User selectedUser = (User)UserList.SelectedItem;
            MessageBoxBuilderModel messageBuilder;

            // Configure and display a confirmation dialog for user deletion.
            messageBuilder = new MessageBoxBuilderModel()
            {
                Message = $"Are you sure you want to delete the user \"{selectedUser.Username}\"?",
                Icon = MessageBoxImage.Warning,
                Title = "User deletion confirmation",
                Button = MessageBoxButton.YesNo
            };

            // Return if the user opts to not delete after the confirmation.
            if (messageBuilder.ShowMessage() != MessageBoxResult.Yes)
                return;

            // Try to remove the user using the API and capture the result.
            RemoveUserResult? result = await Api.RemoveUserAsync(selectedUser.Id);

            // If the result is null, the API call may have failed, and further error handling might be needed here.

            // If a result is received, determine whether it was a successful deletion or not and display the appropriate message.
            if (result != null)
            {
                if (result.Message.MessageType != BaseApi.ServerMessageTypes.Success)
                {
                    // If deletion was not successful, show an error message.
                    messageBuilder = new MessageBoxBuilderModel()
                    {
                        Message = result.Message.Message,
                        Icon = MessageBoxImage.Error,
                        Title = "User Deletion",
                        Button = MessageBoxButton.OK
                    };
                }
                else
                {
                    // If deletion was successful, show an information message.
                    messageBuilder = new MessageBoxBuilderModel()
                    {
                        Message = "User deleted successfully.",
                        Icon = MessageBoxImage.Information,
                        Title = "User Deletion",
                        Button = MessageBoxButton.OK
                    };
                }

                // Display the result message.
                messageBuilder.ShowMessage();

                // Refresh the list of users after the deletion.
                await InitUsersListAsync(true);
            }
        } // Event handler for clicking the button to remove a user.

        private void ViewMark()
        {
            if (MarkList.SelectedItem is not DisplayMark displayMark)
            {
                MessageBoxBuilderModel messageBuilder;

                // Configure and display a confirmation dialog for user deletion.
                messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = $"No mark selected.",
                    Icon = MessageBoxImage.Error,
                    Title = "View Mark",
                    Button = MessageBoxButton.OK
                };

                messageBuilder.ShowMessage();

                return;
            }

            string url = $"https://xmt-school.tiro-finale.com/take_a_test?testId={displayMark.Test.Id}&imposterId={displayMark.Student.Id}";
            Process myProcess = new Process();

            try
            {
                // true is the default, but it is important not to set it to false
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = url;
                myProcess.Start();
            }
            catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
        }

        private void UserSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            // Check if the TextBox contains the placeholder text and clear it if so.
            if (UserSearch.Text == "Search...")
            {
                UserSearch.Text = string.Empty; // Clear the placeholder text to allow for user input.
            }
        } // Event handler for when the UserSearch TextBox gains focus.

        private void TestSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            // Clear the 'Search...' placeholder text when the search box gets focus.
            if (TestSearch.Text == "Search...")
            {
                TestSearch.Text = string.Empty;
            }
        } // Handles the GotFocus event for the TestSearch TextBox.

        private void MarkSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            // Clear the 'Search...' placeholder text when the search box gets focus.
            if (MarkSearch.Text == "Search...")
            {
                MarkSearch.Text = string.Empty;
            }
        }

        private async void UserSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            // Check if the TextBox is empty or contains only white-space, and add placeholder text if so.
            if (string.IsNullOrWhiteSpace(UserSearch.Text))
            {
                UserSearch.Text = "Search..."; // Add the placeholder text.

                // Reinitialize the list of users to show all users since the search field is cleared.
                await InitUsersListAsync(false);
            }
        } // Event handler for when the UserSearch TextBox loses focus.

        private async void TestSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            // Replace the 'Search...' placeholder text when the search box loses focus and is empty.
            if (string.IsNullOrWhiteSpace(TestSearch.Text))
            {
                TestSearch.Text = "Search...";

                // Reinitialize the test list since the search box is empty.
                await InitTestsListAsync(false);
            }
        } // Handles the LostFocus event for the TestSearch TextBox.

        private async void MarkSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            // Replace the 'Search...' placeholder text when the search box loses focus and is empty.
            if (string.IsNullOrWhiteSpace(TestSearch.Text))
            {
                TestSearch.Text = "Search...";

                // Reinitialize the test list since the search box is empty.
                await InitMarksListAsync(false);
            }
		}

		private void TestList_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			// Determine the enable state based on the selection count of TestList.
			bool state = TestList.SelectedItems.Count > 0;

			// Enable or disable the context menu options based on the state.
			ContextTestEdit.IsEnabled = state;
			ContextTestRemove.IsEnabled = state;
		} // Enables context menu options when the right mouse button is released on the TestList,

		// Fixes for better experience
		private void TestList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TestList.UnselectAll();
        } // Unselects all tests when the right mouse button is pressed down on the TestList.

        private void TestList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TestList.UnselectAll();
        } // Unselects all tests when the left mouse button is pressed down on the TestList.

        private void UserList_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Determine the enable state based on the selection count of UserList.
            bool state = UserList.SelectedItems.Count > 0;

            // Enable ContextUserUpdate based on the state.
            ContextUserUpdate.IsEnabled = state;

            // ContextUserRemove availability is determined by both the state and the 
            // enablement of the UserRemoveBtn, indicating if the current user is allowed
            // to remove other users.
            ContextUserRemove.IsEnabled = UserRemoveBtn.IsEnabled & state;
        } // Enables context menu options when the right mouse button is released on the UserList,

        private void UserList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UserList.UnselectAll();
        } // Unselects all users when the left mouse button is pressed down on the UserList.

        private void UserList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            UserList.UnselectAll();
        } // Unselects all users  when the right mouse button is pressed down on the UsersList.

		private void MarkList_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			// Determine the enable state based on the selection count of UserList.
			bool state = MarkList.SelectedItems.Count > 0;

			// Enable ContextUserUpdate based on the state.
			ContextMarkView.IsEnabled = state;
			ContextMarkRemove.IsEnabled = state;
		}

		private void MarkList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			MarkList.UnselectAll();
		}

        private void MarkList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			MarkList.UnselectAll();
		}

		#region Filtering portion
		private readonly ObservableCollection<string> _testFilterOptions = new ObservableCollection<string>
		{
			"Show all",
			"Show ongoing",
			"Show upcoming",
			"Show ended"
		};

		private bool FilterTestsFunc(TestWithAuthorName test)
		{
			bool byOptions(TestWithAuthorName test)
			{
				return TestFilterOptions_ComboBox.SelectedIndex switch
				{
					0 => true,
					1 => test.Test.CanBeTaken(false),
					2 => test.Test.IsUpcoming(),
					3 => test.Test.IsExpired(),
					_ => false
				};
			}

			bool bySearch()
			{
                if (TestSearch.Text == "Search...")
                    return true;
                
				return test.Test.Name.ToLower().Contains(TestSearch.Text.ToLower());
			}

			return byOptions(test) && bySearch();
		}

		private readonly ObservableCollection<string> _markFilterOptions = new ObservableCollection<string>
		{
			"Show all",
			"Show over 55 points",
			"Show below (and) 55 points"
		};

		private bool FilterMarksFunc(DisplayMark mark)
		{
			bool byOptions(DisplayMark mark)
			{
				return MarkFilterOptions_ComboBox.SelectedIndex switch
				{
					0 => true,
					1 => mark.Points > 55,
					2 => mark.Points <= 55,
					_ => false
				};
			}

			bool bySearch()
			{
				if (MarkSearch.Text == "Search...")
					return true;

				return mark.Student.Username.ToLower().Contains(MarkSearch.Text.ToLower());
			}

			return byOptions(mark) && bySearch();
		}

		private readonly ObservableCollection<string> _userFilterOptions = new ObservableCollection<string>
		{
			"Show all",
            $"Show: {UserTypes.Student}",
			$"Show: {UserTypes.Teacher}",
			$"Show: {UserTypes.Administrator}"
		};

		private bool FilterUsersFunc(User user)
		{
			bool byOptions(User user)
			{
				return UserFilterOptions_ComboBox.SelectedIndex switch
				{
					0 => true,
                    1 => user.Type == UserTypes.Student,
                    2 => user.Type == UserTypes.Teacher,
                    3 => user.Type == UserTypes.Administrator,
					_ => false
				};
			}

			bool bySearch()
			{
				if (UserSearch.Text == "Search...")
					return true;

				return user.Username.ToLower().Contains(UserSearch.Text.ToLower());
			}

			return byOptions(user) && bySearch();
		}

		private static void SetFilterOptions<T>(ListView listView, ComboBox comboBox, ObservableCollection<string> options, Predicate<T> filterFunc)
		{
			if (comboBox.SelectedIndex < 0)
			{
				comboBox.DataContext = options;
				comboBox.SelectedIndex = 0;
			}

			CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listView.ItemsSource);
            
            // Convert object to typ when sending request to filter func.
            // It's easier to use later.
			view.Filter = (object obj) => filterFunc((T)obj);
		}

		private void TestSearch_TextChanged(object sender, TextChangedEventArgs e)
		{
            if (TestList.ItemsSource != null)
			    CollectionViewSource.GetDefaultView(TestList.ItemsSource).Refresh();
		}

		private void TestFilterOptions_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (TestList.ItemsSource != null)
				CollectionViewSource.GetDefaultView(TestList.ItemsSource).Refresh();
		}

		private void MarkSearch_SelectionChanged(object sender, RoutedEventArgs e)
		{
			if (MarkList.ItemsSource != null)
				CollectionViewSource.GetDefaultView(MarkList.ItemsSource).Refresh();
		}

		private void MarkFilterOptions_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (MarkList.ItemsSource != null)
				CollectionViewSource.GetDefaultView(MarkList.ItemsSource).Refresh();
		}

		private void UserSearch_TextChanged(object sender, EventArgs e)
		{
			if (UserList.ItemsSource != null)
				CollectionViewSource.GetDefaultView(UserList.ItemsSource).Refresh();
		}
		private void UserFilterOptions_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (UserList.ItemsSource != null)
				CollectionViewSource.GetDefaultView(UserList.ItemsSource).Refresh();
		}
		#endregion
	}
}
