using voto.Context;
using voto.Entities;

namespace voto.Services.Interfaces;

public interface IVotoService
{
    Task<int> Crear(Voto voto);
    Task<List<Resultado>> ObtenerResultados();
}