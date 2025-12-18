namespace pigmentos.API.Models
{
    public class DatabaseSettings
    {
        public string Database { get; set; } = null!;
        public string ColeccionColores { get; set; } = null!;
        public string ColeccionFamiliasQuimicas { get; set; } = null!;
        public string ColeccionPigmentos { get; set; } = null!;

        public DatabaseSettings(IConfiguration unaConfiguracion)
        {
            var configuracion = unaConfiguracion.GetSection("DatabaseSettings");

            Database = configuracion.GetSection("Database").Value!;
            ColeccionColores = configuracion.GetSection("ColorsCollection").Value!;
            ColeccionFamiliasQuimicas = configuracion.GetSection("ChemicalFamiliesCollection").Value!;
            ColeccionPigmentos = configuracion.GetSection("PigmentsCollection").Value!;
        }
    }
}