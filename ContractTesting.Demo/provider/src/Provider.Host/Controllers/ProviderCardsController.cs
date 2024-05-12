﻿using System.Net;
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
            return result != null ? Ok(MapperExtensions.MapDomainModelToContract(result)) : NotFound();
        }
    }