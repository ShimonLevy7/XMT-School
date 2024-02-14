using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;

using XmtSchoolTypes.Login;
using XmtSchoolTypes.Tests;

using XmtSchoolUtils;

namespace XmtSchoolTypes.Users
{
	public enum UserTypes : byte
	{
		Guest = 0,
		Student = 1,
		Teacher = 2,
		Administrator = 100
	};

	public class User
	{
		public const string DefaultAvatarUrl = "https://cdn.discordapp.com/attachments/836004673299021824/1107722671850004570/avatar.png";

		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required(ErrorMessage = "User type is required.")]
		public UserTypes Type { get; set; } = UserTypes.Guest;

		[Required(ErrorMessage = "Username is required.")]
		[StringLength(32, ErrorMessage = "The {0} must not exceed {1} characters.")]
		public string Username { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required.")]
		[StringLength(64, ErrorMessage = "The {0} must not exceed {1} characters.")]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "E-Mail is required.")]
		[StringLength(128, ErrorMessage = "The {0} must not exceed {1} characters.")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Avatar is required.")]
		[StringLength(512, ErrorMessage = "The {0} must not exceed {1} characters.")]
		public string AvatarUrl { get; set; } = string.Empty;

		// Foreign keys.
		[JsonIgnore]
		public List<Token> Tokens { get; set; } = new List<Token>(); // Foreign key between tokens and users.

		[JsonIgnore]
		public List<Test> Tests { get; set; } = new List<Test>(); // Foreign key between tests and authors.

		[JsonIgnore]
		public List<Mark> Marks { get; set; } = new List<Mark>(); // Foreign key between marks and students.

		public User(int Id, UserTypes Type, string Username, string Password, string Email, string AvatarUrl)
		{
			this.Id = Id;
			this.Type = Type;
			this.Username = Username;
			this.Password = Password;
			this.Email = Email;
			this.AvatarUrl = AvatarUrl;
		}

		public bool SanityCheck()
		{
			if (string.IsNullOrEmpty(Username) || Username.Length < 2)
				throw new Exception($"Username minimum character length is 2, received: {Password.Length}");

			if (string.IsNullOrEmpty(Password) || Password.Length < 8)
				throw new Exception($"Password minimum character length is 8, received: {Password.Length}");

			if (string.IsNullOrEmpty(Email) || !Utilities.IsValidEmail(Email))
				throw new Exception($"Invalid E-Mail address, received: {Email}");

			return true;
		}
	}
}
