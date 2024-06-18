using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;

        private readonly ILogger<DeleteOrderCommand> _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository,
                                          IMapper mapper,
                                          ILogger<DeleteOrderCommand> logger)
        {
            this._orderRepository = orderRepository;

            this._logger = logger;
        }
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _orderRepository.GetByIdAsync(request.Id);

            if (orderToDelete == null)
                throw new OrderNotFoundException(nameof(Order), request.Id);

            await _orderRepository.DeleteAsync(orderToDelete);

            _logger.LogInformation($"Order with Id {request.Id} is deleted successfully.");
            return Unit.Value;
        }
    }
}
