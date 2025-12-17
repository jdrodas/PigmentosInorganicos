using Npgsql;
using System.Data;

namespace pigmentos.API.DbContexts
{
    public class PgsqlDbContext(IConfiguration unaConfiguracion)
    {
        private readonly string cadenaConexion = unaConfiguracion.GetConnectionString("pigmentosPL")!;
        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(cadenaConexion);
        }
    }
}