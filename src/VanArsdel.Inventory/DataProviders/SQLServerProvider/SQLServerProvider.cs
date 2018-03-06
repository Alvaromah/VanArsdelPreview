using System;

using VanArsdel.Data.Services;

namespace VanArsdel.Inventory.Providers
{
    public class SQLServerProvider : SQLBaseProvider
    {
        public SQLServerProvider(string connectionString)
            : base(new SQLServerDataService(connectionString))
        {
        }
    }
}
