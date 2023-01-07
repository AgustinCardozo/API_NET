using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace API_Demo.Database
{
    //Permite comunicar con la DB
    public class DapperContext
    {
        private readonly IConfiguration configuration;

        public DapperContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IDbConnection CreateConnection() => new SqlConnection(configuration.GetConnectionString(Consts.ConfigKeys.CONN_DB));
    }
}
