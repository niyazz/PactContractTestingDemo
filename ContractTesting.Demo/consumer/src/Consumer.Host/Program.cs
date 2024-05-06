using Consumer.Domain;
using Consumer.Integration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ConsumerCardService>();
builder.Services.AddHttpClient<ProviderCardIntegration>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ProviderAppBaseUrl"]);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();