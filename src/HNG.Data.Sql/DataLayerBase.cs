using System.Data.Common;
using System.Data.SqlClient;
using HNG.Abstractions.Models;

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
            return new SqlConnection(AppSettings.ConnectionStrings.ConnectionString);
        }
    }
}
