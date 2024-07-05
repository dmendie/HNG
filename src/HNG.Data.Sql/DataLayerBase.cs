using HNG.Abstractions.Models;
using Npgsql;
using System.Data.Common;

namespace HNG.Data.Sql
{
    public class DataLayerBase
    {
        readonly AppSettings AppSettings;
        public DataLayerBase(AppSettings appSettings)
        {
            AppSettings = appSettings;
        }

        protected DbConnection GetDefaultConnection()
        {
            return new NpgsqlConnection(AppSettings.ConnectionStrings.ConnectionString);
        }
    }
}
