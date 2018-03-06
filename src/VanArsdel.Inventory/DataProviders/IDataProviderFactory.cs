using System;

namespace VanArsdel.Inventory.Providers
{
    public interface IDataProviderFactory
    {
        IDataProvider CreateDataProvider();
    }
}
