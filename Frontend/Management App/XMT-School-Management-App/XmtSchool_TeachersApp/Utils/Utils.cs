using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace XmtSchool_TeachersApp.Utils
{
	internal class Utils
	{
		internal static async Task<ImageSource?> GetImageSourceFromUrlAsync(string ImageUrl)
		{
			HttpClient httpClient = new HttpClient();

			byte[] imageData;

			try
			{
				imageData = await httpClient.GetByteArrayAsync(ImageUrl);
			}
			catch
			{
				return null;
			}

			BitmapImage bitmapImage = new BitmapImage();

			using (MemoryStream memoryStream = new MemoryStream(imageData))
			{
				memoryStream.Position = 0;
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.StreamSource = memoryStream;
				bitmapImage.EndInit();
			}

			return bitmapImage;
		}
	}
}
