using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Candidato> _votos = new();

app.MapGet("/votos", () => TypedResults.Ok(_votos));
app.MapPost("/votos", ([FromBody] Candidato _nuevoCandidato) => {
    _nuevoCandidato.Id = _votos.Count + 1;
    _votos.Add(_nuevoCandidato);
    return TypedResults.Ok(_nuevoCandidato);
});
app.MapPut("/votos/{id}", ([FromRoute] int id, [FromBody] Candidato _candidatoActualizado) =>  {
    foreach (var _candidato in _votos)
    {
        if (_candidato.Id == id) 
        {
            _candidato.VotanteId = _candidatoActualizado.VotanteId;
            _candidato.CandidatoId = _candidatoActualizado.CandidatoId;
            _candidato.Partido = _candidatoActualizado.Partido;
        }
    }
    return TypedResults.Ok(_candidatoActualizado);
});
app.MapDelete("/votos/{id}", ([FromRoute] int id) => {
    _votos = _votos.Where(c => c.Id != id).ToList();
    return TypedResults.Ok();
});

app.Run();

public class Candidato
{
    public int Id { get; set; }
    public int VotanteId { get; set; }
    public int CandidatoId { get; set; }
    public bool EsNulo { get; set; }
    public bool EsImpugnado { get; set; }
}
