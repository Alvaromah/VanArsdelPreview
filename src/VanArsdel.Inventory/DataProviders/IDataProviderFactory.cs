using System;

namespace VanArsdel.Inventory.Data
{
    public interface IDataProviderFactory
    {
        IDataProvider CreateDataProvider();
    }
}
