using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Entities;
using Common.Logging.Correlation;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers.V2;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BasketController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly DiscountGrpcService _discountGrpcService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<BasketController> _logger;
    private readonly ICorrelationIdGenerator _correlationIdGenerator;

    public BasketController(IMediator mediator,
                            DiscountGrpcService discountGrpcService,
                            IPublishEndpoint publishEndpoint,
                            ILogger<BasketController> logger,
                            ICorrelationIdGenerator correlationIdGenerator)
    {
        _mediator = mediator;
        this._discountGrpcService = discountGrpcService;
        this._publishEndpoint = publishEndpoint;
        this._logger = logger;
        this._correlationIdGenerator = correlationIdGenerator;
        _logger.LogInformation("CorrelationId {correlationId}:", _correlationIdGenerator.Get());
    }

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckoutV2 basketCheckout)
    {
        //get existing basket with user name
        var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
        var basket = await _mediator.Send(query);

        if (basket is null)
        {
            return BadRequest();
        }

        var eventMsg = BasketMapper.Mapper.Map<BasketCheckoutEventV2>(basketCheckout);

        eventMsg.TotalPrice = basket.TotalPrice;
        eventMsg.CorrelationId = _correlationIdGenerator.Get();

        await _publishEndpoint.Publish(eventMsg);

        var deleteCmd = new DeleteBasketByUserNameCommand(basketCheckout.UserName);

        await _mediator.Send(deleteCmd);

        return Accepted();
    }
}
