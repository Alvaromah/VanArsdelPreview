using System;

namespace VanArsdel.Inventory.Providers
{
    public class DataProviderFactory : IDataProviderFactory
    {
        public IDataProvider CreateDataProvider()
        {
            // TODO: Return selected DataProvider in configuration
            // TODO: Get connection string from AppSettings
            return new SQLiteDataProvider(null);
        }
    }
}
