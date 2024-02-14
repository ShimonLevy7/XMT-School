using System.Text.RegularExpressions;

namespace XmtSchoolUtils
{
	public static class Utilities
	{
		private static readonly Random _Random = new Random();

		// For lists.
		public static void Shuffle<T>(this IList<T> ListToShuffle)
		{
			int n = ListToShuffle.Count;

			while (n > 1)
			{
				// Get random key and reduce the current `n` by one.
				int k = _Random.Next(n--);

				// Swap the values.
				(ListToShuffle[n], ListToShuffle[k]) = (ListToShuffle[k], ListToShuffle[n]);
			}
		}

		// For arrays.
		public static void Shuffle<T>(this T[] ArrayToShuffle)
		{
			int n = ArrayToShuffle.Length;

			while (n > 1)
			{
				// Get random key and reduce the current `n` by one.
				int k = _Random.Next(n--);

				// Swap values.
				(ArrayToShuffle[k], ArrayToShuffle[n]) = (ArrayToShuffle[n], ArrayToShuffle[k]);
			}
		}

		public static bool IsValidEmail(string Email)
		{
			// A more comprehensive regular expression for email validation.
			string pattern = @"^(?=.{1,256}$)[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$";

			return Regex.IsMatch(Email, pattern);
		}

		public static bool IsValidImageUrl(string ImageUrl)
		{
			// Define a regular expression pattern for common image file extensions.
			string pattern = @"^https?://.+\.(jpg|jpeg|png|gif|bmp|svg|webp)$";

			return Regex.IsMatch(ImageUrl, pattern, RegexOptions.IgnoreCase);
		}
	}
}
