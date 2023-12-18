using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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

        app.Run();
    }
}
