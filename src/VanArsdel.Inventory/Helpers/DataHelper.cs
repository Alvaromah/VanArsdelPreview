using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory
{
    public class DataHelper
    {
        static public DataHelper Current => ThreadSafeSingleton<DataHelper>.Instance;

        public IList<CountryCodeModel> CountryCodes { get; private set; }
        public IList<OrderStatusModel> OrderStatus { get; private set; }
        public IList<PaymentTypeModel> PaymentTypes { get; private set; }
        public IList<ShipperModel> Shippers { get; private set; }
        public IList<TaxTypeModel> TaxTypes { get; private set; }

        public async Task InitializeAsync(IDataProviderFactory providerFactory)
        {
            using (var dataProvider = providerFactory.CreateDataProvider())
            {
                CountryCodes = await dataProvider.GetCountryCodesAsync();
                OrderStatus = await dataProvider.GetOrderStatusAsync();
                PaymentTypes = await dataProvider.GetPaymentTypesAsync();
                Shippers = await dataProvider.GetShippersAsync();
                TaxTypes = await dataProvider.GetTaxTypesAsync();
            }
        }

        public string GetCountry(string id)
        {
            return CountryCodes.Where(r => r.CountryCodeID == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetOrderStatus(int id)
        {
            return OrderStatus.Where(r => r.Status == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetPaymentType(int id)
        {
            return PaymentTypes.Where(r => r.PaymentTypeID == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetShipper(int id)
        {
            return Shippers.Where(r => r.ShipperID == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetTaxDesc(int id)
        {
            return TaxTypes.Where(r => r.TaxTypeID == id).Select(r => $"{r.Rate} %").FirstOrDefault();
        }
        public decimal GetTaxRate(int id)
        {
            return TaxTypes.Where(r => r.TaxTypeID == id).Select(r => r.Rate).FirstOrDefault();
        }
    }
}
