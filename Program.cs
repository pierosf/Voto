using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VotoDb>(opt => opt.UseInMemoryDatabase("Votos"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.MapGet("/votos", (VotoDb _db) => TypedResults.Ok(_db.Votos.ToList()));
app.MapPost("/votos", ([FromBody] Voto _nuevoVoto, VotoDb _db) => {
    _db.Votos.Add(_nuevoVoto);
    _db.SaveChanges();
    return TypedResults.Ok(_nuevoVoto.Id);
});

app.Run();

public class VotoDb : DbContext
{
    public VotoDb(DbContextOptions<VotoDb> options) : base(options) { }

    public DbSet<Voto> Votos => Set<Voto>();
}

public class Voto
{
    public int Id { get; set; }
    public int VotanteId { get; set; }
    public int? CandidatoId { get; set; }
}
