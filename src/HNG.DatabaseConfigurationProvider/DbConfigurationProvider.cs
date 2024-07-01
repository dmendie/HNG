using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HNG.DatabaseConfigurationProvider
{
    public class DbConfigurationProvider : ConfigurationProvider
    {
        private readonly Timer? _refreshTimer = null;
        public DbConfigurationSource Source { get; }

        public DbConfigurationProvider(DbConfigurationSource source)
        {
            Source = source;

            if (Source.RefreshInterval.HasValue)
            {
                _refreshTimer = new Timer(_ => ReadDatabaseSettings(true), null, Timeout.Infinite, Timeout.Infinite);
            }
        }

        public override void Load()
        {
            if (string.IsNullOrWhiteSpace(Source.ConnectionString))
            {
                return;
            }

            Data = ReadDatabaseSettings(false);

            if (_refreshTimer != null && Source.RefreshInterval.HasValue)
            {
                _refreshTimer.Change(Source.RefreshInterval.Value, Source.RefreshInterval.Value);
            }
        }

        private IDictionary<string, string?> ReadDatabaseSettings(bool isReload)
        {
            using var connection = new SqlConnection(Source.ConnectionString);
            var command = new SqlCommand("Configuration_List", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@Entity", Source.AppName));

            var settings = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
            try
            {
                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    try
                    {
                        settings[reader.GetString("Key")] = reader.GetString("Value");
                    }
                    catch (Exception readerEx)
                    {
                        System.Diagnostics.Debug.WriteLine(readerEx);
                    }
                }
                reader.Close();

                return settings;
            }
            catch (Exception sqlEx)
            {
                System.Diagnostics.Debug.WriteLine(sqlEx);
            }

            return settings;
        }

        public void Dispose()
        {
            _refreshTimer?.Change(Timeout.Infinite, Timeout.Infinite);
            _refreshTimer?.Dispose();
        }
    }
}
