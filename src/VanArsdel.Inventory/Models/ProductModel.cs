using System;

using Windows.UI.Xaml.Media.Imaging;

namespace VanArsdel.Inventory.Models
{
    public class ProductModel : ModelBase
    {
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
        public BitmapImage PictureBitmap { get; set; }

        public byte[] Thumbnail { get; set; }
        public BitmapImage ThumbnailBitmap { get; set; }
    }
}
