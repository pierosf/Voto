using voto.Context;

namespace voto.Repository.Interfaces;

public interface IVotoRepository
{
    Task<int> Crear(Voto voto);
    Task<List<Voto>> Obtener();
}