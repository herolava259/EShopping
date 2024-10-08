﻿using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    public class BasketController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IMediator mediator, 
                                DiscountGrpcService discountGrpcService,
                                IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            this._discountGrpcService = discountGrpcService;
            this._publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        [Route("[action]/{userName}", Name = "GetBasketByUserName")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);

            var basket = await _mediator.Send(query);

            return Ok(basket);
        }

        [HttpPost("CreateBasket")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody] CreateShoppingCartCommand createShoppingCartCommand)
        {
            foreach(var item in  createShoppingCartCommand.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);

                item.Price -= coupon.Amount;
            }

            var basket = await _mediator.Send(createShoppingCartCommand);

            return Ok(basket);
        }

        [HttpGet]
        [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> DeleteBasket(string userName)
        {
            var command = new DeleteBasketByUserNameCommand(userName);

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            //get existing basket with user name
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);

            if(basket is null)
            {
                return BadRequest();
            }

            var eventMsg = BasketMapper.Mapper.Map<BasketCheckout>(basketCheckout);

            eventMsg.TotalPrice = basket.TotalPrice;

            await _publishEndpoint.Publish(eventMsg);

            var deleteCmd = new DeleteBasketByUserNameCommand(basketCheckout.UserName);

            await _mediator.Send(deleteCmd);

            return Accepted();
        }
    }
}
