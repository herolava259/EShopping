using Amazon.Runtime.Internal.Util;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetAllProductsHandler> _logger;

        public GetAllProductsHandler(IProductRepository productRepository, ILogger<GetAllProductsHandler> logger)
        {
            this._productRepository = productRepository;
            this._logger = logger;
        }
        public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProducts(request.CatalogSpecParams);

            var productResponseList = ProductMapper.Mapper.Map<Pagination<ProductResponse>>(productList);
            _logger.LogDebug("Received product list.Total count: {productList}", productResponseList.Count);
            return productResponseList;
        }
    }
}
