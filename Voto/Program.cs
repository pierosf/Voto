using System.Reflection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using voto.Context;
using voto.ExternalServices;
using voto.ExternalServices.Interfaces;
using voto.Middlewares;
using voto.Repository;
using voto.Repository.Interfaces;
using voto.Services;
using voto.Services.Interfaces;

namespace voto;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connection = new SqliteConnection("DataSource=votos.db");
        connection.Open();
        builder.Services.AddDbContext<VotoDb>(opt => opt.UseSqlite(connection));

        builder.Services.AddControllers();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {   
                Version = "v1",
                Title = "Voto API",
                Description = "Una API para registrar votos y obtener resultados para votaciones",
            });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Services.AddScoped<IVotoService, VotoService>();
        builder.Services.AddScoped<IVotanteService, VotanteService>();
        builder.Services.AddScoped<ICandidatoService, CandidatoService>();
        builder.Services.AddScoped<IVotoRepository,VotoRepository>();
        builder.Services.AddScoped<ILogRepository,LogRepository>();
        

        builder.Services.AddHttpClient();

        var app = builder.Build();

        app.UseDatabaseLoggingMiddleware();
        app.UseExceptionHandlerMiddleware();
        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.Run();
    }
}
