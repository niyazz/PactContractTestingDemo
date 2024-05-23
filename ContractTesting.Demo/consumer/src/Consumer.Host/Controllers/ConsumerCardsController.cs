using Consumer.Domain;
using Consumer.Domain.Models.V1;
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
    /// Заказать карту
    /// </summary>
    /// <param name="userId">Идентификатор клиента</param>
    /// <param name="request">Тело запроса</param>
    [HttpPost("{userId}")]
    public async Task<ActionResult<CardsScreenResponse>> OrderCard(string userId, [FromBody] CardOrderRequest request)
    {
        var result = await _consumerCardService.OrderCard(userId, request);
        return result.Success ? Ok(result.Data) : BadRequest();
    }
}