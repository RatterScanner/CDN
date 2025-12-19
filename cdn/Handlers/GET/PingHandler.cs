namespace cdn.Handlers.Get;

public class PingHandler
{
    public IResult Handle()
    {
        return Results.Ok(new { status = "OK" });
    }
}
