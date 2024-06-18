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
    public static class BrandContextSeed
    {
        public static void SeedData(IMongoCollection<ProductBrand> brandCollection) 
        {
            bool checkBrands = brandCollection.Find(b => true).Any();
#if DEBUG
            string path = Path.Combine("bin/Debug/net6.0/Data", "SeedData", "brands.json");

#else
            string path = Path.Combine("Data", "SeedData", "brands.json");
#endif
            if (!checkBrands)
            {
                var brandData = File.ReadAllText(path);
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                if(brands != null)
                {
                    foreach (var item in brands)
                    {
                        brandCollection.InsertOneAsync(item);
                    }
                }
            }

        }
    }
}
