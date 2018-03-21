using System;

using VanArsdel.Data.Services;

namespace VanArsdel.Inventory.Providers
{
    public class SQLServerDataProvider : SQLBaseProvider
    {
        public SQLServerDataProvider(string connectionString)
            : base(new SQLServerDataService(connectionString))
        {
        }
    }
}
