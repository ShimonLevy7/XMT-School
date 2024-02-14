using Microsoft.EntityFrameworkCore;

using XmtSchoolTypes.Login;
using XmtSchoolTypes.Tests;
using XmtSchoolTypes.Users;

namespace XmtSchoolDatabase
{
	public class XmtSchoolDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Token> Tokens { get; set; }
		public DbSet<Test> Tests { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<Answer> Answers { get; set; }
		public DbSet<Mark> Marks { get; set; }

		public XmtSchoolDbContext(DbContextOptions<XmtSchoolDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().ToTable("Users");
			modelBuilder.Entity<User>()
				.HasMany(u => u.Marks) // A user has many marks
				.WithOne(u => u.User) // But a mark only has one user
				.HasForeignKey(m => m.UserId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Cascade); // When a user is deleted, delete all marks

			modelBuilder.Entity<Token>().ToTable("Tokens");
			modelBuilder.Entity<Token>()
				.HasOne(t => t.User) // A token has one user
				.WithMany(u => u.Tokens) // But a user has many tokens
				.HasForeignKey(t => t.UserId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Cascade); // When a user is deleted, delete all tokens

			modelBuilder.Entity<Test>().ToTable("Tests");
			modelBuilder.Entity<Test>()
				.HasOne(t => t.Author) // A test has one author
				.WithMany(u => u.Tests) // An author has many tests
				.HasForeignKey(t => t.AuthorUserId)
				.IsRequired()
				.OnDelete(DeleteBehavior.NoAction); // Don't delete tests when the author user is deleted

			modelBuilder.Entity<Question>().ToTable("Questions");
			modelBuilder.Entity<Question>()
				.HasOne(q => q.Test) // A question has one test
				.WithMany(t => t.Questions) // A test has many questions
				.HasForeignKey(t => t.TestId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Cascade); // When a test is deleted, delete all questions related to it

			modelBuilder.Entity<Answer>().ToTable("Answers");
			modelBuilder.Entity<Answer>()
				.HasOne(a => a.Question) // An answer has one question
				.WithMany(q => q.Answers) // A question has many answers
				.HasForeignKey(a => a.QuestionId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Cascade); // When a question is deleted, delete all answers related to it

			modelBuilder.Entity<Mark>().ToTable("Marks")
				.HasMany(m => m.SelectedAnswers) // A mark has many selected answers
				.WithOne()
				.IsRequired()
				.OnDelete(DeleteBehavior.Cascade); // When a mark is deleted, delete all selected answers related to it

			modelBuilder.Entity<SelectedAnswer>().ToTable("SelectedAnswers");
			modelBuilder.Entity<SelectedAnswer>()
				.HasOne(sa => sa.Question) // A selected answer has one question
				.WithMany()
				.HasForeignKey(sa => sa.QuestionId)
				.IsRequired()
				.OnDelete(DeleteBehavior.NoAction); // Nothing to cascade to
			modelBuilder.Entity<SelectedAnswer>()
				.HasOne(sa => sa.Answer) // A selected answer has one answer
				.WithMany()
				.HasForeignKey(sa => sa.AnswerId)
				.IsRequired()
				.OnDelete(DeleteBehavior.NoAction);  // Nothing to cascade to

			base.OnModelCreating(modelBuilder);
		}

		/// <summary>
		/// Adds a token to the database and saves it.
		/// </summary>
		/// <param name="NewToken"></param>
		public void AddToken(Token NewToken)
		{
			Tokens.Add(NewToken);

			SaveChanges();
		}

		/// <summary>
		/// Finds a token (type) based on a token string and a user id.
		/// </summary>
		/// <param name="TokenString"></param>
		/// <param name="UserId"></param>
		/// <returns></returns>
		public Token? FindToken(string TokenString, uint UserId)
		{
			return Tokens.Where(x => x.TokenString == TokenString && x.UserId == UserId).FirstOrDefault();
		}

		/// <summary>
		/// Tries to remove a token from the database and returns `true` if success or `false` if failed.
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public bool RemoveToken(Token Token)
		{
			Token? tokenToRemove = Tokens.Where(x => x.TokenString == Token.TokenString).FirstOrDefault();

			if (tokenToRemove == null)
				return false;

			Tokens.Remove(tokenToRemove);

			SaveChanges();

			return true;
		}

		/// <summary>
		/// Marks a token in the database as "used".
		/// This means the token expiration date will be extended.
		/// </summary>
		/// <param name="tokenString"></param>
		public void MarkTokenUsed(string tokenString)
		{
			Token? usedToken = Tokens.Where(x => x.TokenString == tokenString).FirstOrDefault();

			if (usedToken == null)
				return;

			usedToken.LastUsed = DateTime.UtcNow;

			SaveChanges();
		}

		/// <summary>
		/// Finds a user based on their token string.
		/// </summary>
		/// <param name="TokenString"></param>
		/// <returns></returns>
		public User? GetUserByTokenString(string TokenString)
		{
			Token? token = Tokens.Where(x => x.TokenString == TokenString).FirstOrDefault();

			if (token == null)
				return null;

			User? user = Users.Where(x => x.Id == token.UserId).FirstOrDefault();

			if (user == null)
				return null;

			return user;
		}

		/// <summary>
		/// Finds a user by their id.
		/// </summary>
		/// <param name="UserId"></param>
		/// <returns></returns>
		public User? GetUserById(int UserId)
		{
			return Users.Where(user => user.Id == UserId).FirstOrDefault();
		}

		/// <summary>
		/// Finds a user by their username and password.
		/// </summary>
		/// <param name="Username"></param>
		/// <param name="Password"></param>
		/// <returns></returns>
		public User? GetUserByUsernameAndPassword(string Username, string Password)
		{
			return Users.Where(user => user.Username == Username && user.Password == Password).FirstOrDefault();
		}

		/// <summary>
		/// Receive a list of all the tests in the database.
		/// </summary>
		/// <param name="WithNestedData">To include all collections within a test? (Typically Questions & Answers)</param>
		/// <returns>A list of Tests</returns>
		public List<Test> GetAllTests(bool WithNestedData)
		{
			if (!WithNestedData)
				return Tests.ToList();

			return Tests
				.Include(test => test.Author) // Include the author for every Test
				.Include(test => test.Questions) // Include the Questions related to each Test
					.ThenInclude(question => question.Answers) // Include the Answers related to each Question
				.ToList(); // As a list
		}

		/// <summary>
		/// Find a singular test by its id
		/// </summary>
		/// <param name="TestId"></param>
		/// <param name="WithNestedData">To include all collections within a test? (Typically Questions & Answers)</param>
		/// <returns></returns>
		public Test? GetTestByTestId(int TestId, bool WithNestedData)
		{
			if (!WithNestedData)
				return Tests.Where(t => t.Id == TestId).FirstOrDefault();

			return Tests.Where(t => t.Id == TestId).Include(test => test.Questions) // Include the Questions related to each Test
				.ThenInclude(question => question.Answers) // Include the Answers related to each Question
				.FirstOrDefault();
		}

		/// <summary>
		/// Get a list of marks for a user (and include the selected answers collection) by their user id.
		/// </summary>
		/// <param name="UserId"></param>
		/// <returns></returns>
		public List<Mark> GetMarksForUser(int UserId)
		{
			return Marks.Where(m => m.UserId == UserId)
				.Include(mark => mark.SelectedAnswers)
				.ToList();
		}

		/// <summary>
		/// Adds a test to the database and saves it.
		/// This will run a sanity check on the test, make sure to try catch this method.
		/// </summary>
		/// <param name="testToAdd"></param>
		/// <exception cref="Exception">Exception may be thrown if test or collections are invalid.</exception>
		public void AddTest(Test testToAdd)
		{
			testToAdd.SanityCheck(); // May not be handled, must be handled externally.

			Tests.Add(testToAdd);

			SaveChanges(); // Save changes to the database
		}

		/// <summary>
		/// Adds a user to the database and saves it.
		/// </summary>
		/// <param name="userToAdd"></param>
		public void AddUser(User userToAdd)
		{
			Users.Add(userToAdd);

			SaveChanges();
		}
	}
}
