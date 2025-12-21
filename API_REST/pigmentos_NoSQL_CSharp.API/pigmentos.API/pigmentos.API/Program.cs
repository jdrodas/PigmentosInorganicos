using Asp.Versioning;
using Microsoft.OpenApi;
using pigmentos.API.DbContexts;
using pigmentos.API.Interfaces;
using pigmentos.API.Repositories;
using pigmentos.API.Services;

var builder = WebApplication.CreateBuilder(args);

//Aqui agregamos los servicios requeridos

// ***************************************************************************
// --- Configuración del DB Context --
// ***************************************************************************

builder.Services.AddSingleton<MongoDbContext>();

// ***************************************************************************
// --- Configuración de los repositorios --
// ***************************************************************************

builder.Services.AddScoped<IEstadisticaRepository, EstadisticaRepository>();
builder.Services.AddScoped<IColorRepository, ColorRepository>();
builder.Services.AddScoped<IFamiliaRepository, FamiliaRepository>();
builder.Services.AddScoped<IPigmentoRepository, PigmentoRepository>();

// ***************************************************************************
// --- Configuración de los servicios asociados  --
// ***************************************************************************

builder.Services.AddScoped<EstadisticaService>();
builder.Services.AddScoped<ColorService>();
builder.Services.AddScoped<FamiliaService>();
builder.Services.AddScoped<PigmentoService>();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "pigmentos.API v1 - MongoDB",
        Description = "API para la gestión de Información sobre Pigmentos Inorgánicos"
    });
});

// ***************************************************************************
// --- Configuración del versionamiento para el API  --
// ***************************************************************************

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("api-version"),
        new QueryStringApiVersionReader("api-version")
    );
}
)
    .AddMvc()
    .AddApiExplorer(setup =>
    {
        setup.GroupNameFormat = "'v'VVV";
        setup.SubstituteApiVersionInUrl = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        // Configuración manual de cada endpoint
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "pigmentos.API v1");
    }
    );
}

//Modificamos el encabezado de las peticiones para ocultar el web server utilizado
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Server", "PigmentosServer");
    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();