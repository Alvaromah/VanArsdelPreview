using System;
using System.Threading.Tasks;

using Windows.UI.Xaml.Media.Imaging;

using VanArsdel.Inventory.Data;

namespace VanArsdel.Inventory.Models
{
    public class CategoryModel : ModelBase
    {
        public CategoryModel()
        {
        }
        public CategoryModel(Category source)
        {
            CategoryID = source.CategoryID;
            Name = source.Name;
            Description = source.Description;
            Picture = source.Picture;
            Thumbnail = source.Thumbnail;
        }

        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public byte[] Thumbnail { get; set; }

        private BitmapImage _pictureBitmap = null;
        public BitmapImage PictureBitmap
        {
            get { return _pictureBitmap; }
            set { Set(ref _pictureBitmap, value); }
        }

        private BitmapImage _thumbnailBitmap = null;
        public BitmapImage ThumbnailBitmap
        {
            get { return _thumbnailBitmap; }
            set { Set(ref _thumbnailBitmap, value); }
        }

        public async Task LoadAsync()
        {
            PictureBitmap = await BitmapTools.LoadBitmapAsync(Picture);
            ThumbnailBitmap = await BitmapTools.LoadBitmapAsync(Thumbnail);
        }
    }
}
