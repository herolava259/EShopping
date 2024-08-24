using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.API.EventBusConsumer;

public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<BasketOrderingConsumer> _logger;

    public BasketOrderingConsumer(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumer> logger)
    {
        this._mediator = mediator;
        this._mapper = mapper;
        this._logger = logger;
    }
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        using var scope = _logger.BeginScope("Consuming Basket Checkout Event for {correlationId}",
                context.CorrelationId);
        var cmd = _mapper.Map<CheckoutOrderCommand>(context.Message);
        _ = await _mediator.Send(cmd);
        _logger.LogInformation($"Basket checkout event completed!!!");
    }
}
