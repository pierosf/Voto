using voto.Context;
using voto.Repository.Interfaces;

namespace voto.Repository;

public class LogRepository : ILogRepository
{
    private readonly VotoDb _db;

    public LogRepository(VotoDb db)
    {
        _db = db;
    }

    public async Task CreateLog(Log log)
    {
        await _db.Logs.AddAsync(log);
        await _db.SaveChangesAsync();
    }
}