using System.Net;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Provider.Contracts.Models;
using Provider.Domain;
using Provider.Host.Helpers;

namespace Provider.Host.Controllers;

/*
 *
 *{
  "providerName": "Demo.Provider",
  "providerApplicationVersion": "1.1.0",
  "success": false,
  "verificationDate": "2024-07-16T10:59:09+00:00",
  "testResults": [{
      "interactionId": "45f04c85450a74e86ee3532f64fc610fa62b3410",
      "success": true
    }, {
      "interactionId": "a34f508ad02d3fdc02128a8bc1d17423121a2155",
      "success": true
    }, {
      "interactionId": "f94fb21777492253d725d93eef28d2f8344f44aa",
      "mismatches": [{
          "attribute": "body",
          "description": "Expected body Present(89 bytes, application/json) but was empty",
          "identifier": "/"
        }, {
          "attribute": "status",
          "description": "expected 200 but was 404"
        }, {
          "attribute": "header",
          "description": "Expected a header 'Content-Type' but was missing",
          "identifier": "Content-Type"
        }],
      "success": false
    }, {
      "interactionId": "e4304a59a374b803855951c157a7282e9302bfc5",
      "success": true
    }],
  "verifiedBy": {
    "implementation": "Pact-Rust",
    "version": "1.1.19"
  }
}
 * 
 */

    [Route("api/provider/cards")]
    public class ProviderCardsController : ControllerBase
    {
        private readonly ICardAccountsRepository _cardAccountsRepository;
        public ProviderCardsController(ICardAccountsRepository cardAccountsRepository)
        {
            _cardAccountsRepository = cardAccountsRepository;
        }
    
        /// <summary>
        /// Получить карточные счета клиента
        /// </summary>
        /// <param name="userId">Идентификатор клиента</param>
         [HttpGet("accounts/{userId}")]
         [ProducesResponseType(typeof(UserCardAccountsResponse), (int)HttpStatusCode.OK)]
         [ProducesResponseType((int)HttpStatusCode.NotFound)]
         public async Task<ActionResult<UserCardAccountsResponse>> GetUserCardAccounts(string userId)
        {
            var result = await _cardAccountsRepository.GetCardAccountsByUserId(userId);
            return result != null ? Ok(MapperExtensions.MapUserCardAccounts(result)) : NotFound();
        }

        /// <summary>
        /// Заказ новой карты
        /// </summary>
        /// <param name="userId">Идентификатор клиента</param>
        /// <param name="accountId">Идентификатор счёта</param>
        /// <param name="request">Тело запроса</param>
        [HttpPost("{userId}")]
        public async Task<ActionResult<CardInfoResponse>> CreateCardOrder(string userId, [FromBody] CreateCardOrderRequest request)
        {
            var result = await _cardAccountsRepository.AddCard(userId, "acidef8ef642-3cab-4f70-9c12-c9757e698ad1", request.IsNamed);
            return result != null ? Ok(MapperExtensions.MapCardInfo(result)) : NotFound();
        }
        
        /// <summary>
        /// Отправить событие о готовности карты
        /// </summary>
        /// <param name="userId">Идентификатор клиента</param>
        [HttpPost("order-satisfied/{userId}")]
        public async Task<ActionResult> SendCardOrderSatisfiedEvent(string userId)
        {
            var advancedBus = RabbitHutch.CreateBus("host=localhost", s =>
            {
                s.EnableConsoleLogger();
                s.EnableSystemTextJson();
            }).Advanced;
            var exchange = await advancedBus.ExchangeDeclareAsync("SpecialExchangeName", "direct");
            var message = new Message<CardOrderSatisfiedEvent>(new CardOrderSatisfiedEvent
            {
                UserId = userId,
                CardCode = Random.Shared.Next(100)
            });
            await advancedBus.PublishAsync(exchange, "super-routing-key", false, message);
            return Ok();
        }
    }