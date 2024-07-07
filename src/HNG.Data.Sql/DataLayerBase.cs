using HNG.Abstractions.Models;
using Npgsql;
using System.Data.Common;
using System.Data.SqlClient;

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
            if (AppSettings.Settings.UsePostGresDBEnterprise)
            {
                return new SqlConnection(AppSettings.ConnectionStrings.ConnectionString);
            }
            else
            {
                return new NpgsqlConnection(AppSettings.ConnectionStrings.ConnectionString);
            }
        }
    }
}
