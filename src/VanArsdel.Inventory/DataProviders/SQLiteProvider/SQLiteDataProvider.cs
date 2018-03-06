using System;

using VanArsdel.Data.Services;

namespace VanArsdel.Inventory.Providers
{
    public class SQLiteDataProvider : SQLBaseProvider
    {
        public SQLiteDataProvider(string connectionString)
            : base(new SQLiteDataService(connectionString))
        {
        }
    }
}
