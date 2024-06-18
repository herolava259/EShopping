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
    public class TypeContextSeed
    {
        public static void SeedData(IMongoCollection<ProductType> brandCollection)
        {
            bool checkTypes = brandCollection.Find(b => true).Any();

            
#if DEBUG
            string path = Path.Combine("bin/Debug/net6.0/Data", "SeedData", "types.json");

#else
            string path = Path.Combine("Data", "SeedData", "types.json");
#endif

            if (!checkTypes)
            {
                var typesData = File.ReadAllText(path);
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                if (types != null)
                {
                    foreach (var item in types)
                    {
                        brandCollection.InsertOneAsync(item);
                    }
                }
            }

        }
    }
}
