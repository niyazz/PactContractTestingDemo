using Microsoft.AspNetCore.Mvc;
using Provider.Contracts.Models;
using Provider.Domain;
using Provider.Domain.Models;

namespace Provider.Host.Controllers;

    [Route("api/cards")]
    public class CardsController : ControllerBase
    {
        private readonly CardAccountsRepository _cardAccountsRepository;
        public CardsController(CardAccountsRepository cardAccountsRepository)
        {
            _cardAccountsRepository = cardAccountsRepository;
        }
    
        /// <summary>
        /// Получить экран карт клиента
        /// </summary>
        /// <param name="userId">Идентификатор клиента</param>
         [HttpGet("accounts/{userId}")]
         public async Task<ActionResult<CardAccountInfoResponse[]>> GetCardsScreen(string userId)
        {
            var result = await _cardAccountsRepository.GetCardAccountsByUserId(userId);
            return result != null ? Ok(MapDomainModelToContract(result)) : NotFound();
        }

        private CardAccountInfoResponse[] MapDomainModelToContract(CardAccountInfo[] accountsInfo) =>
            accountsInfo.Select(account => new CardAccountInfoResponse
            {
                Id = account.Id,
                OpenDate = account.OpenDate,
                CloseDate = account.CloseDate,
               // ClientInfo = account.,
               Cards = account.Cards.Select(card => new CardInfoResponse
               {
                   Id = card.Id,
                   OpenDate = card.OpenDate,
                   CloseDate = card.CloseDate,
                   ExpiryDate = card.ExpiryDate,
                   Balance = card.Balance,
                   IsNamed = card.IsNamed,
                   State = card.State
               }).ToArray()
            }).ToArray();
    }