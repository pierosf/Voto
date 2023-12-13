using voto.Helpers;

namespace voto.Context;

public class Log
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public TypeEnum Type { get; set; }
    public required string Message { get; set; }
}