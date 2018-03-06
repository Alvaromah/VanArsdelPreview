using System;

namespace VanArsdel.Data.Services
{
    public class SQLServerDataService : DataServiceBase
    {
        public SQLServerDataService(string connectionString)
            : base(new SQLServerDb(connectionString))
        {
        }
    }
}
