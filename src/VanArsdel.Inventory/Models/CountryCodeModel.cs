using System;

using VanArsdel.Inventory.Data;

namespace VanArsdel.Inventory.Models
{
    public class CountryCodeModel : ModelBase
    {
        public CountryCodeModel()
        {
        }
        public CountryCodeModel(CountryCode source)
        {
            CountryCodeID = source.CountryCodeID;
            Name = source.Name;
        }

        public string CountryCodeID { get; set; }
        public string Name { get; set; }
    }
}
