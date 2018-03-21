using System;

namespace VanArsdel.Inventory.Providers
{
    public class DataProviderFactory : IDataProviderFactory
    {
        public IDataProvider CreateDataProvider()
        {
            // TODO: Return selected DataProvider in configuration
            return new SQLiteDataProvider(AppSettings.SQLiteConnectionString);
            //return new SQLServerDataProvider(AppSettings.SQLServerConnectionString);
        }
    }
}
