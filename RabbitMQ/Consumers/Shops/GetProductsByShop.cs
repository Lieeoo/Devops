﻿using MassTransit;
using RtuItLab.Infrastructure.MassTransit.Requests.Shops;
using Shops.Domain.Services;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Consumers.Shops
{
    public class GetProductsByShop : ShopsBaseConsumer, IConsumer<GetProductsRequest>
    {
        public GetProductsByShop(IShopsService shopsService) : base(shopsService)
        {
        }

        public async Task Consume(ConsumeContext<GetProductsRequest> context)
        {
            var order = await ShopsService.GetProductsByShop(context.Message.ShopId);
            var response = new GetProductsResponse
            {
                Products = order?.ToList(),
                Success = order != null
            };
            await context.RespondAsync(response);
        }
    }
}
