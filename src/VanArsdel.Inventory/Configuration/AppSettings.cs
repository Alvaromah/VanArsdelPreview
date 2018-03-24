using System;
using System.IO;

using Windows.ApplicationModel;

namespace VanArsdel.Inventory
{
    public class AppSettings
    {
        const string DB_NAME = "VanArsdel";
        const string DB_VERSION = "0.10";
        const string DB_BASEURL = "https://vanarsdelinventory.blob.core.windows.net/database";

        static AppSettings()
        {
            Current = new AppSettings();
        }

        static public AppSettings Current { get; }

        public string Version
        {
            get
            {
                var ver = Package.Current.Id.Version;
                return $"{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}";
            }
        }

        static public readonly string DatabasePath = "Database";
        static public readonly string DatabaseName = $"{DB_NAME}.{DB_VERSION}.db";
        static public readonly string DatabasePattern = $"{DB_NAME}.{DB_VERSION}.pattern.db";
        static public readonly string DatabaseFileName = Path.Combine(DatabasePath, DatabaseName);
        static public readonly string DatabaseUrl = $"{DB_BASEURL}/{DatabaseName}";

        static public readonly string SQLiteConnectionString = $"Data Source={DatabaseFileName}";
        static public readonly string SQLServerConnectionString = @"Data Source=.\SQLExpress;Initial Catalog=VanArsdelDb;Integrated Security=SSPI";
    }
}
