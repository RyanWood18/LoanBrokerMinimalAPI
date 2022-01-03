using Microsoft.Extensions.Azure;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSendGrid((s, o) =>
{
    var config = s.GetService<IConfiguration>();
    o.ApiKey = config.GetValue<string>("SendGridApiKey");
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();