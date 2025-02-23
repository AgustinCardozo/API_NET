using System.Data;
using System.Data.SqlClient;
using API_Demo.Configurations;
using Microsoft.Extensions.Options;

namespace API_Demo.Database
{
    //Permite comunicar con la DB
    public class DapperContext
    {
        //private readonly IConfiguration configuration;
        private readonly IOptions<ConnStrOptions> options;

        public DapperContext(IOptions<ConnStrOptions> options)
        {
            //this.configuration = configuration;
            this.options = options;
        }

        //public IDbConnection CreateConnection() => new SqlConnection(configuration.GetConnectionString(Consts.ConfigKeys.CONN_DB));
        public IDbConnection CreateConnection() => new SqlConnection(ReplaceDatabase(options.Value.DatabaseConnection));

        private string ReplaceDatabase(string dbConection) =>
            dbConection.Replace(Consts.DATABASE, options.Value.Database);
    }
}
