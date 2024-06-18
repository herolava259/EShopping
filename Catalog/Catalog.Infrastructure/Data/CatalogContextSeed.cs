using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool checkProducts = productCollection.Find(b => true).Any();

#if DEBUG
            string path = Path.Combine("bin/Debug/net6.0/Data", "SeedData", "products.json");

#else
            string path = Path.Combine("Data", "SeedData", "products.json");
#endif

            if (!checkProducts)
            {
                var productsData = File.ReadAllText(path);
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products != null)
                {
                    foreach (var item in products)
                    {
                        productCollection.InsertOneAsync(item);
                    }
                }
            }

        }
    }
}
