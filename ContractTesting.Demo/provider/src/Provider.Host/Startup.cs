using EasyNetQ;
using EasyNetQ.DI;
using Provider.Contracts.Models;
using Provider.Domain;
namespace Provider.Host;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    class SimpleTypeNameSerializer : ITypeNameSerializer
    {
        public string Serialize(Type type) => type.Name;

        public Type DeSerialize(string typeName)
        {
            return Type.GetType("Provider.Contracts.Models.CardOrderApplicationEvent, Provider.Contracts");
        }
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc().AddControllersAsServices();
        services.AddScoped<ICardAccountsRepository, CardAccountsRepository>();
        services.AddSwaggerGen();
        
        var advanced = RabbitHutch.CreateBus("host=localhost:5672;username=guest;password=guest", s =>
        {
            s.EnableConsoleLogger();
            s.EnableSystemTextJson();
            s.Register<ITypeNameSerializer, SimpleTypeNameSerializer>();
        }).Advanced;
        var exchange = advanced.ExchangeDeclare("SpecialExchangeName", "direct");
        var queue = advanced.QueueDeclare("SpecialQueueName");
        advanced.Bind(exchange, queue, routingKey: "super-routing-key");
        advanced.Consume<CardOrderApplicationEvent>(queue, (message, _) =>
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Новый заказ: {message.Body.UserId}, {message.Body.CardCode}, {message.Body.ApplicationDate}");
            }));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}