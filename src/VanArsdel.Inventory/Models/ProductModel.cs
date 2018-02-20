using System;
using System.Threading.Tasks;

using Windows.UI.Xaml.Media.Imaging;

using VanArsdel.Inventory.Data;

namespace VanArsdel.Inventory.Models
{
    public class ProductModel : ModelBase
    {
        public ProductModel()
        {
        }
        public ProductModel(Product source)
        {
            ProductID = source.ProductID;
            CategoryID = source.CategoryID;
            SubCategoryID = source.SubCategoryID;
            Name = source.Name;
            Description = source.Description;
            Size = source.Size;
            Color = source.Color;
            Gender = source.Gender;
            ListPrice = source.ListPrice;
            DealerPrice = source.DealerPrice;
            TaxType = source.TaxType;
            Discount = source.Discount;
            DiscountStartDate = source.DiscountStartDate;
            DiscountEndDate = source.DiscountEndDate;
            StockUnits = source.StockUnits;
            SafetyStockLevel = source.SafetyStockLevel;
            StartDate = source.StartDate;
            EndDate = source.EndDate;
            CreatedOn = source.CreatedOn;
            LastModifiedOn = source.LastModifiedOn;
            Picture = source.Picture;
            Thumbnail = source.Thumbnail;
        }

        public string ProductID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Gender { get; set; }
        public decimal ListPrice { get; set; }
        public decimal DealerPrice { get; set; }
        public int TaxType { get; set; }
        public decimal Discount { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public int StockUnits { get; set; }
        public int SafetyStockLevel { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
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
