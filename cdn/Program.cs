var builder = WebApplication.CreateBuilder(args);

// Initialize openAPI, this can be used for automatic documentation.
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.MapGet("/ping", () => Results.Ok(new { status = "OK" }));

app.Run();