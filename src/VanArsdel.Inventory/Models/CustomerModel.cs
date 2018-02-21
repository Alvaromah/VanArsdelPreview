using System;
using System.Threading.Tasks;

using Windows.UI.Xaml.Media.Imaging;

using VanArsdel.Inventory.Data;

namespace VanArsdel.Inventory.Models
{
    public class CustomerModel : ModelBase
    {
        public CustomerModel()
        {
        }
        public CustomerModel(Customer source)
        {
            Source = source;
            CopyValues(Source);
        }

        public Customer Source { get; private set; }

        public long CustomerID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Education { get; set; }
        public string Occupation { get; set; }
        public decimal? YearlyIncome { get; set; }
        public string MaritalStatus { get; set; }
        public int? TotalChildren { get; set; }
        public int? ChildrenAtHome { get; set; }
        public bool? IsHouseOwner { get; set; }
        public int? NumberCarsOwned { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public byte[] Picture { get; set; }
        public byte[] Thumbnail { get; set; }

        public string FullName => $"{FirstName} {LastName}";

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

        private void CopyValues(Customer source)
        {
            CustomerID = source.CustomerID;
            Title = source.Title;
            FirstName = source.FirstName;
            MiddleName = source.MiddleName;
            LastName = source.LastName;
            Suffix = source.Suffix;
            Gender = source.Gender;
            EmailAddress = source.EmailAddress;
            AddressLine1 = source.AddressLine1;
            AddressLine2 = source.AddressLine2;
            City = source.City;
            Region = source.Region;
            CountryCode = source.CountryCode;
            PostalCode = source.PostalCode;
            Phone = source.Phone;
            BirthDate = source.BirthDate;
            Education = source.Education;
            Occupation = source.Occupation;
            YearlyIncome = source.YearlyIncome;
            MaritalStatus = source.MaritalStatus;
            TotalChildren = source.TotalChildren;
            ChildrenAtHome = source.ChildrenAtHome;
            IsHouseOwner = source.IsHouseOwner;
            NumberCarsOwned = source.NumberCarsOwned;
            CreatedOn = source.CreatedOn;
            LastModifiedOn = source.LastModifiedOn;
            Picture = source.Picture;
            Thumbnail = source.Thumbnail;
        }

        public void CommitChanges()
        {
            Source.Title = Title;
            Source.FirstName = FirstName;
            Source.MiddleName = MiddleName;
            Source.LastName = LastName;
            Source.Suffix = Suffix;
            Source.Gender = Gender;
            Source.EmailAddress = EmailAddress;
            Source.AddressLine1 = AddressLine1;
            Source.AddressLine2 = AddressLine2;
            Source.City = City;
            Source.Region = Region;
            Source.CountryCode = CountryCode;
            Source.PostalCode = PostalCode;
            Source.Phone = Phone;
            Source.BirthDate = BirthDate;
            Source.Education = Education;
            Source.Occupation = Occupation;
            Source.YearlyIncome = YearlyIncome;
            Source.MaritalStatus = MaritalStatus;
            Source.TotalChildren = TotalChildren;
            Source.ChildrenAtHome = ChildrenAtHome;
            Source.IsHouseOwner = IsHouseOwner;
            Source.NumberCarsOwned = NumberCarsOwned;
            Source.LastModifiedOn = DateTime.UtcNow;
            Source.Picture = Picture;
            Source.Thumbnail = Thumbnail;
        }

        public void UndoChanges()
        {
            CopyValues(Source);
        }
    }
}
