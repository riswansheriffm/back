using BackPack.Dependency.Library.Responses;
using Npgsql;

namespace BackPack.Library.Repositories
{
    public abstract class GenericRepository()
    {        
        #region DatabaseConnection
        public static NpgsqlConnection DatabaseConnection()
        {
            NpgsqlConnection dbConnection = new(GlobalApplicationProperty.DBConnection);

            return dbConnection;
        }
        #endregion
    }
}
