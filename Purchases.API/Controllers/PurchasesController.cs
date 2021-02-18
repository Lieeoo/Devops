﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Purchases.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        /// <summary>
        /// Получить список покупок
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ICollection<string>> GetHistory()
        {
            return null;
        }
        /// <summary>
        /// Добавить транзакцию
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AddTransaction()
        {
            return null;
        }
        /// <summary>
        /// Обновить транзакцию, если можно
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<string> UpdateTransaction()
        {
            return null;
        }
        /// <summary>
        /// Получить определенную транзакцию
        /// </summary>
        /// <param name="transationId"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> GetTransaction(string transationId)
        {
            return null;
        }

    }
}
