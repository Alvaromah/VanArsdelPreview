﻿using System;

namespace VanArsdel.Inventory.Providers
{
    public enum DataProviderType
    {
        SQLite,
        SQLServer,
        WebAPI
    }

    public class DataProviderFactory : IDataProviderFactory
    {
        public IDataProvider CreateDataProvider()
        {
            switch (AppSettings.Current.DataProvider)
            {
                case DataProviderType.SQLite:
                    return new SQLiteDataProvider(AppSettings.Current.SQLiteConnectionString);

                case DataProviderType.SQLServer:
                    return new SQLServerDataProvider(AppSettings.Current.SQLServerConnectionString);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
