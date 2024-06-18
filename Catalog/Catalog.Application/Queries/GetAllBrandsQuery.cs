using Catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class GetAllBrandsQuery: IRequest<IList<BrandResponse>>
    {
        public string Id { get; set; }

        public string Name { get; set;}
    }
}
