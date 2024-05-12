using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Moq;
using Provider.Host;

namespace Provider.ContractTests;

public class TestHost : IDisposable
{
    private readonly IHostBuilder _serverBuilder;
    private IHost _server;
    private bool _disposedValue;

    public TestHost(Uri serverUri)
    {
        _serverBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls(serverUri.ToString());
                webBuilder.UseStartup<Startup>();
            }); 
    }
    
    public void ReplaceServiceWithMock<TService>(Mock<TService> mock, ServiceLifetime serviceLifetime)
        where TService : class
    {
        _serverBuilder.ConfigureServices(services =>
        {
            services.RemoveAll<TService>();
            services.Add(new ServiceDescriptor(typeof(TService), _ => mock.Object, serviceLifetime));
        });
    }

    public void BuildThenStart()
    {
        _server = _serverBuilder.Build();
        _server.StartAsync();
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _server.StopAsync().GetAwaiter().GetResult();
                _server.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose() => Dispose(true);
}
