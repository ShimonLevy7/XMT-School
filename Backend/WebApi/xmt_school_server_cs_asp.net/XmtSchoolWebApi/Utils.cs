using System.Security.Cryptography;
using System.Text;

using static XmtSchoolWebApi.Types.BaseApi;

namespace XmtSchoolWebApi
{
    public static class Utils
	{
		/// <summary>
		/// Turn a problem type into a readable string.
		/// </summary>
		/// <param name="ProblemType"></param>
		/// <returns></returns>
		public static string GetProblem(ProblemTypes ProblemType)
		{

			return $"Error {(ushort)ProblemType}: {ProblemTypeToString[ProblemType]}";
		}

		/// <summary>
		/// Convert a password string into a password hash.
		/// </summary>
		/// <param name="Password"></param>
		/// <returns></returns>
		public static string GetMD5HashFromPassword(string Password)
		{
			// Convert the input string to a byte array and compute the hash.
			byte[] inputBytes = Encoding.UTF8.GetBytes(Password);
			byte[] hashBytes = MD5.HashData(inputBytes);

			// Convert the byte array to a hexadecimal string.
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < hashBytes.Length; i++)
			{
				sb.Append(hashBytes[i].ToString("x2"));
			}

			return sb.ToString();
		}
	}
}
