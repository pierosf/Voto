using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ServiceReference;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.EnvironmentName != "Development")
{
    var connection = new SqliteConnection("DataSource=votos.db");
    connection.Open();
    builder.Services.AddDbContext<VotoDb>(opt => opt.UseSqlite(connection));
}
else
{
    builder.Services.AddDbContext<VotoDb>(opt => opt.UseInMemoryDatabase("Votos"));
}


var app = builder.Build();

HttpClient client = new HttpClient();

app.MapGet("/votos", (VotoDb _db) => TypedResults.Ok(_db.Votos.ToList()));
app.MapPost("/votos", async ([FromBody] Voto _nuevoVoto, VotoDb _db) => {
    var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"http://localhost:5000/votantes/{_nuevoVoto.VotanteId}"));
    var response = await client.SendAsync(request, CancellationToken.None);
    response.EnsureSuccessStatusCode();
    var votante = JsonSerializer.Deserialize<Votante>(await response.Content.ReadAsStringAsync());
    DateTime FechaNacimiento = DateTime.ParseExact(votante.FechaNacimiento, "yyyy-MM-dd", CultureInfo.InvariantCulture);

    var soapServiceChannel = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
    var respuesta = await soapServiceChannel.SubtractAsync(DateTime.Now.Year, FechaNacimiento.Year);

    if (respuesta > 16)
    {
        _db.Votos.Add(_nuevoVoto);
        _db.SaveChanges();
        return TypedResults.Ok(_nuevoVoto.Id);
    }
    else 
    {
        throw new CustomException("El votante no tiene la edad para votar", 500);
    }
});

app.Use(async (context, next) => {
    var _db = context.RequestServices.GetRequiredService<VotoDb>();
    _db.Logs.Add(new Log() {
        Message = $"{context.Request.Path} - {context.Request.Method}",
        Date = new DateTime(),
        Type = TypeEnum.REQUEST
    });
    await next.Invoke();
    _db.Logs.Add(new Log() {
        Message = $"{context.Request.Path} - {context.Request.Method} - {context.Response.StatusCode}",
        Date = new DateTime(),
        Type = TypeEnum.RESPONSE
    });
    _db.SaveChanges();
});

app.Use(async (context, next) => {
    try 
    {
        await next.Invoke();
    }
    catch(CustomException ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new ErrorEntity() {Message = ex.Message, StatusCode = ex.StatusCode});
    }
});


app.Run();



public class VotoDb : DbContext
{
    public VotoDb(DbContextOptions<VotoDb> options) : base(options) { 
        Database.EnsureCreated();
    }

    public DbSet<Voto> Votos => Set<Voto>();
    public DbSet<Log> Logs => Set<Log>();
}

public class Voto
{
    public int Id { get; set; }
    public int VotanteId { get; set; }
    public int? CandidatoId { get; set; }
}

public class Votante
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("fechaNacimiento")]
    public required string FechaNacimiento { get; set; }
}


public class Log
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public TypeEnum Type { get; set; }
    public required string Message { get; set; }
}

public enum TypeEnum
{ 
    REQUEST,
    RESPONSE
}

public class CustomException : Exception 
{
    public CustomException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
    public int StatusCode {get; set;}
}

public class ErrorEntity
{
    public int StatusCode { get; set; }
    public required string Message { get; set; }
}