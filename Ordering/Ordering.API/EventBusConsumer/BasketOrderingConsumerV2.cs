using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.API.EventBusConsumer;

public class BasketOrderingConsumerV2 : IConsumer<BasketCheckoutEventV2>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<BasketOrderingConsumerV2> _logger;

    public BasketOrderingConsumerV2(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumerV2> logger)
    {
        this._mediator = mediator;
        this._mapper = mapper;
        this._logger = logger;
    }
    public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
    {
        using var scope = _logger.BeginScope("Consuming Basket Checkout Event for {correlationId}",
                context.CorrelationId);
        var cmd = _mapper.Map<CheckoutOrderCommand>(context.Message);
        PopulateAddressDetails(cmd);
        _ = await _mediator.Send(cmd);
        _logger.LogInformation($"Basket checkout event completed!!!");
    }

    private void PopulateAddressDetails(CheckoutOrderCommand cmd)
    {
        cmd.FirstName = "Farrer";
        cmd.LastName = "Le";
        cmd.EmailAddress = "chopperman259@gmail.com";
        cmd.AddressLine = "Vietnam";
        cmd.Country = "Vietnamese";
        cmd.ZipCode = "100000";
        cmd.State = "HN";
        cmd.PaymentMethod = 1;
        cmd.CardName = "Visa";
        cmd.CardNumber = "1234567890123456";
        cmd.Expiration = "12/25";
        cmd.CVV = "123";
    }
}
