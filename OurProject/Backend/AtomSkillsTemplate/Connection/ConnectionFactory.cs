using Dapper;
using Npgsql;
using System.Data;
using AtomSkillsTemplate.Connection.Interface;
using Microsoft.Extensions.Configuration;

namespace AstomSkillsTemplate.Connection
{
    public class ConnectionFactory : IConnectionFactory
    {
        public string connString = "";
        private readonly IConfiguration Configuration;
        public ConnectionFactory(IConfiguration configuration)
        {
            
            Configuration = configuration;
            connString = Configuration["ConnectionString"];
            
            
        }
        public IDbConnection GetConnection()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            return conn;
        }
    }
}
