using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

namespace VanArsdel.Inventory
{
    static public class BitmapTools
    {
        static public async Task<BitmapImage> LoadBitmapAsync(byte[] bytes)
        {
            var bitmap = new BitmapImage();
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(bytes.AsBuffer());
                stream.Seek(0);
                await bitmap.SetSourceAsync(stream);
            }
            return bitmap;
        }
    }
}
