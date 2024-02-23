using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using XmtSchoolTypes.Users;

namespace XmtSchoolTypes.Login
{
	public class Token
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[ForeignKey(nameof(UserId))]
		public int UserId { get; set; } // The user this token points to.
		public string TokenString { get; set; }
		public DateTime LastUsed { get; set; }

		public User? User { get; set; } // Foreign key between tokens and users.

		public Token(int Id, int UserId, string TokenString, DateTime LastUsed)
		{
			this.Id = Id;
			this.UserId = UserId;
			this.TokenString = TokenString;
			this.LastUsed = LastUsed;
		}

		/// <summary>
		/// Generate a token for a user.
		/// </summary>
		/// <param name="UserId">The user ID</param>
		public Token(int UserId)
		{
			this.UserId = UserId;

			TokenString = Guid.NewGuid().ToString(); // Consider using JWT in the future.
			LastUsed = DateTime.UtcNow;
		}
	}
}
