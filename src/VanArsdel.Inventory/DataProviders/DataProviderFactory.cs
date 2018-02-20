using System;

namespace VanArsdel.Inventory.Data
{
    public class DataProviderFactory : IDataProviderFactory
    {
        public IDataProvider CreateDataProvider()
        {
            // TODO: Return selected DataProvider in configuration
            return new LocalDataProvider();
        }
    }
}
