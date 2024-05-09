using Microsoft.Extensions.Logging;
using NetwiseProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add logging services with specific console logging configuration
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders(); // Clears default logging providers
    loggingBuilder.AddSimpleConsole(options =>
    {
        options.IncludeScopes = false; // Exclude scopes
        options.SingleLine = true;
        options.TimestampFormat = "HH:mm:ss "; // Include only the hour
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICatFactsClient, CatFactsClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting(); // Make sure to include routing middleware before controllers

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();