using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

List<Voto> _votos = new();

app.MapGet("/votos", () => TypedResults.Ok(_votos));
app.MapPost("/votos", ([FromBody] Voto _nuevoVoto) => {
    _nuevoVoto.Id = _votos.Count + 1;
    _votos.Add(_nuevoVoto);
    return TypedResults.Ok(_nuevoVoto);
});

app.Run();

public class Voto
{
    public int Id { get; set; }
    public int VotanteId { get; set; }
    public int? CandidatoId { get; set; }
}
