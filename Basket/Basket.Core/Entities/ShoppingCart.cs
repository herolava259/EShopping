using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Core.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }

        public List<ShoppingCartItem> Items { get; set; } = new();
        public ShoppingCart()
        {
            
        }

        public ShoppingCart(string userName, List<ShoppingCartItem> items)
        {
            UserName = userName;
            Items = items;

        }

        //public decimal TotalPrice { get; set; }
    }
}
