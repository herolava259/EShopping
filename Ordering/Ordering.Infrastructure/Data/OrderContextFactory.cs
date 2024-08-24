using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
            var optionBuilders = new DbContextOptionsBuilder<OrderContext>();

            optionBuilders.UseSqlServer("Data Source=OrderDb");


            return new OrderContext(optionBuilders.Options);

            
        }
    }
}
