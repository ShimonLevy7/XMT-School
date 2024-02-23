using System;
using System.IO;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using XmtSchool_TeachersApp.Api;
using XmtSchool_TeachersApp.Cache;
using XmtSchool_TeachersApp.Utils;

using static XmtSchoolWebApi.Types.ResultTypes.UsersResultTypes;

namespace XmtSchool.TeachersAppViews
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		private static readonly string PathToDataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
		private static readonly string PathToUserDataFile = Path.Combine(PathToDataDirectory, "user.dat");

		public LoginWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (UseSavedLogin())
			{
				Login_Button.Focus();

				return;
			}

			Username_TextBox.Focus();
		}

		private async void Login_Button_Click(object sender, RoutedEventArgs e)
		{
			IsEnabled = false;

			bool success = await AttemptLoginAsync(Username_TextBox.Text, Password_TextBox.Password);

			if (!success)
			{
				IsEnabled = true;
			}
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private bool UseSavedLogin()
		{
			if (!Directory.Exists(PathToDataDirectory))
				Directory.CreateDirectory(PathToDataDirectory);

			if (!File.Exists(PathToUserDataFile))
				return false;

			string[] fileLines = File.ReadAllLines(PathToUserDataFile);

			if (fileLines.Length < 1)
				return false;

			Username_TextBox.Text = fileLines[0];
			RememberMe_CheckBox.IsChecked = true;

			return true;
		}

		private async Task<bool> AttemptLoginAsync(string Username, string Password)
		{
			switch (RememberMe_CheckBox.IsChecked)
			{
				case true:
				{
					File.WriteAllLines(PathToUserDataFile, new string[] { Username });

					break;
				}
				case false:
				{
					if (File.Exists(PathToUserDataFile)) // Delete the cached data.
					{
						File.Delete(PathToUserDataFile);
					}

					break;
				}
			}

			bool loginSuccess = await Api.LoginAsync(Username, Password);

			if (!loginSuccess)
				return false;

			GetUserResultData? getUserResult = await Api.GetUserByTokenStringAsync();

			if (getUserResult == null)
				return false;

			CurrentUser.Id = getUserResult.Id;
			CurrentUser.Username = getUserResult.Username;
			CurrentUser.AvatarUrl = getUserResult.AvatarUrl;
			CurrentUser.AvatarImage = await Utils.GetImageSourceFromUrlAsync(CurrentUser.AvatarUrl ?? $"https://ui-avatars.com/api/?name={CurrentUser.Username.Replace(' ', '+')}?size=4096")
				?? await Utils.GetImageSourceFromUrlAsync($"https://ui-avatars.com/api/?name={CurrentUser.Username.Replace(' ', '+')}?size=4096");

			CurrentUser.Email = getUserResult.Email;
			CurrentUser.Type = getUserResult.Type;

			LaunchMainWindow();

			return true;
		}

		private void LaunchMainWindow()
		{
			MainWindow mainWindow = new MainWindow();

			mainWindow.Show();

			Close();
		}
    }
}
