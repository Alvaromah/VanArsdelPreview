using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Inventory.Data;
using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory
{
    public class DataHelper
    {
        static public DataHelper Current { get; }

        static DataHelper()
        {
            Current = new DataHelper();
        }

        public IList<CountryCodeModel> CountryCodes { get; private set; }

        public async Task InitializeAsync(IDataProviderFactory providerFactory)
        {
            using (var dataProvider = providerFactory.CreateDataProvider())
            {
                var countryCodes = await dataProvider.GetCountryCodesAsync();
                CountryCodes = countryCodes.Select(r => new CountryCodeModel(r)).ToList();
            }
        }

        public string GetCountry(string id)
        {
            return CountryCodes.Where(r => r.CountryCodeID == id).Select(r => r.Name).FirstOrDefault();
        }
    }
}
