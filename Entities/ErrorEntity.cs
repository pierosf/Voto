namespace voto.Entities;


public class ErrorEntity
{
    public int StatusCode { get; set; }
    public required string Message { get; set; }
}