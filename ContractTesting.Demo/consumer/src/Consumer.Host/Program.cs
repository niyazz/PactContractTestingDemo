using Consumer.Domain;
using Consumer.Domain.Models.V1;
using Consumer.Integration;
using EasyNetQ;
using EasyNetQ.DI;

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

#region CardOrderSatisfiedEvent Consumer

var advanced = RabbitHutch.CreateBus("host=localhost:5672;username=guest;password=guest", s =>
{
    s.EnableConsoleLogger();
    s.EnableSystemTextJson();
    s.Register<ITypeNameSerializer, SimpleTypeNameSerializer>();
}).Advanced;
var exchange = advanced.ExchangeDeclare("SpecialExchangeName", "direct");
var queue = advanced.QueueDeclare("SpecialQueueName");
advanced.Bind(exchange, queue, routingKey: "super-routing-key");
advanced.Consume<CardOrderSatisfiedEvent>(queue, (message, _) =>
    Task.Factory.StartNew(() =>
    {
        Console.WriteLine($"Ваша карта готова: {message.Body.UserId}, {message.Body.CardCode}");
    }));

#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

class SimpleTypeNameSerializer : ITypeNameSerializer
{
    public string Serialize(Type type) => type.Name;
    public Type DeSerialize(string typeName) => typeof(CardOrderSatisfiedEvent);
}   