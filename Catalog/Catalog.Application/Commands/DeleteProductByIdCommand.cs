using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class DeleteProductByIdCommand: IRequest<bool>
    {
        public string Id { get; set; }

        public DeleteProductByIdCommand(string id)
        {
            Id = id;
        }
    }
}
