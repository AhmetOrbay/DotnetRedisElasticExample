using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turkai.Data.RabbitMq;
using Turkai.Model.Entity;

namespace Turkai.Data
{
    public class RabbitMqRepo : IRabbitMqRepo
    {
        private readonly TurkaiDbContext _turkaiDb;

        public RabbitMqRepo(TurkaiDbContext turkaiDb)
        {
            _turkaiDb = turkaiDb;
        }

        public async Task WriteDbContext(List<Product> product)
        {
            try
            {
                var ProdId = product.Select(x => x.Id);
                var Ids = _turkaiDb.Products
                            .Where(x => ProdId.Contains(x.Id))
                            .Select(x=>x.Id);
                var DbCheckTrue = product
                                .Where(x => Ids.Any() && Ids.Contains(x.Id)).ToList();
                var DbCheckFalse = product
                                        .Where(x => !Ids.Any() || !Ids.Contains(x.Id)).ToList();
                if (DbCheckTrue.Any())
                {
                    await Update(DbCheckTrue);
                }
                else
                {
                    await _turkaiDb.Products.AddRangeAsync(DbCheckFalse.DistinctBy(x=>x.Id));
                    await _turkaiDb.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task Update(List<Product> product)
        {
            try
            {
                foreach (var p in product.DistinctBy(x => x.Id))
                {
                    _turkaiDb.Attach(p);
                    _turkaiDb.Entry(p).State = EntityState.Modified;
                }

                await _turkaiDb.SaveChangesAsync();
            }
            catch (Exception)
            {

            }
           
        }
    }
}
