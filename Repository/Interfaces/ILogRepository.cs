using voto.Context;

namespace voto.Repository.Interfaces;

public interface ILogRepository
{
    Task CreateLog(Log log);
}