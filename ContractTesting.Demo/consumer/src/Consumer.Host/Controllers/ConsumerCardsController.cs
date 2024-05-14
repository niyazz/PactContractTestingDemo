using Consumer.Domain;
using Consumer.Domain.Models.V1;
using Consumer.Integration.ProviderContracts.V1;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;

namespace Consumer.Host.Controllers;

[ApiController]
[Route("api/consumer/cards")]
public class ConsumerCardsController : ControllerBase
{
    private readonly ConsumerCardService _consumerCardService;
    public ConsumerCardsController(ConsumerCardService consumerCardService)
    {
        _consumerCardService = consumerCardService;
    }
    
    /// <summary>
    /// Получить экран карт клиента
    /// </summary>
    /// <param name="userId">Идентификатор клиента</param>
    [HttpGet("{userId}")]
    public async Task<ActionResult<CardsScreenResponse>> GetCardsScreen(string userId)
    {
        var result = await _consumerCardService.GetCardsScreen(userId);
        return result.Success ? Ok(result.Data) : BadRequest();
    }
    
    /// <summary>
    /// Отправить заявление на заказ карты
    /// </summary>
    /// <param name="userId">Идентификатор клиента</param>
    [HttpPost("application/{userId}")]
    public async Task<ActionResult<CardsScreenResponse>> SendCardOrderApplication(string userId)
    {
        var advancedBus = RabbitHutch.CreateBus("host=localhost").Advanced;
        var exchange = await advancedBus.ExchangeDeclareAsync("SpecialExchangeName", "direct");
        var message = new Message<CardOrderApplicationEvent>(new CardOrderApplicationEvent
        {
            UserId = userId,
            CardCode = Random.Shared.Next(100),
            ApplicationDate = DateTime.Now
        });
        await advancedBus.PublishAsync(exchange, "super-routing-key", false, message);
        return Ok();
    }
}