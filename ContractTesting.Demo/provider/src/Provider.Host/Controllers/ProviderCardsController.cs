using System.Net;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Provider.Contracts.Models;
using Provider.Domain;
using Provider.Host.Helpers;

namespace Provider.Host.Controllers;

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
        public async Task<ActionResult<CardInfoResponse>> CreateCardOrder(string userId, 
            [FromQuery] string accountId, [FromBody] CreateCardOrderRequest request)
        {
            var result = await _cardAccountsRepository.AddCard(userId, accountId, request.IsNamed);
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