using Consumer.Domain;
using Consumer.Domain.Models.V1;
using Microsoft.AspNetCore.Mvc;

namespace Consumer.Host.Controllers;

[ApiController]
[Route("api/cards")]
public class CardsController : ControllerBase
{
    private readonly ConsumerCardService _consumerCardService;
    public CardsController(ConsumerCardService consumerCardService)
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
        return result.Success ? Ok(result.Data) : NotFound();
    }
}