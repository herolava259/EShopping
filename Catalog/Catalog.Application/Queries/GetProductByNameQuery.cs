﻿using Amazon.Runtime.Internal;
using Catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class GetProductByNameQuery : IRequest<IList<ProductResponse>>
    {
        public string Name { get; init; }

        public GetProductByNameQuery(string name)
        {
            Name = name;
        }
    }
}
