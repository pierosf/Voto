using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

HttpClient client = new HttpClient();

app.MapGet("/votos", (VotoDb _db) => TypedResults.Ok(_db.Votos.ToList()));
app.MapPost("/votos", async ([FromBody] Voto _nuevoVoto, VotoDb _db) => {
    var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"http://localhost:5000/votantes/{_nuevoVoto.VotanteId}"));
    request.Headers.Accept.Clear();
    var response = await client.SendAsync(request, CancellationToken.None);
    response.EnsureSuccessStatusCode();
    var votante = JsonSerializer.Deserialize<Votante>(await response.Content.ReadAsStringAsync());
    DateTime FechaNacimiento = DateTime.ParseExact(votante.FechaNacimiento, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    if ((DateTime.Now.Year - FechaNacimiento.Year) > 16)
    {
        _db.Votos.Add(_nuevoVoto);
        _db.SaveChanges();
        return TypedResults.Ok(_nuevoVoto.Id);
    }
    else 
    {
        throw new Exception("Es menor y no puede votar");
    }
});

app.Run();


public class VotoDb : DbContext
{
    public VotoDb(DbContextOptions<VotoDb> options) : base(options) { 
        Database.EnsureCreated();
    }

    public DbSet<Voto> Votos => Set<Voto>();
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
