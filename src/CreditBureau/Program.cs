using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
var ssnRegex = new Regex("^\\d{3}-\\d{2}-\\d{4}$");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/GetCreditScore/{ssn}/", (string ssn) =>
    {
        if (!ssnRegex.IsMatch(ssn))
        {
            return Results.BadRequest("SSN was not in the expected format");
        }

        return Results.Ok(new {SSN = ssn, Score = GetRandom(300, 900), History = GetRandom(1, 30)});
    })
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);

app.Run();

int GetRandom(int min, int max)
{
    var random = new Random();
    return random.Next(min, max);
}