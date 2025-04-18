using BackPack.Dependency.Library.Helpers;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BackPack.Tenant.Library.Repositories
{
    public abstract class GenericRepository(IConfiguration configuration)
    {
        #region DatabaseConnection
        public NpgsqlConnection DatabaseConnection()
        {
            Aes256Helper aes256Helper = new(configuration);
            string dbConnectionString = aes256Helper.Aes256Decryption(configuration.GetSection("DatabaseSettings").GetSection("ConnectionString").Value!);
            NpgsqlConnection dbConnection = new(dbConnectionString);
            
            return dbConnection;
        }
        #endregion
    }
}
