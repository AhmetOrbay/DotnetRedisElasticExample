using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turkai.Model.Entity;

namespace Turkai.Data.RabbitMq
{
    public interface IRabbitMqRepo
    {
        Task WriteDbContext(List<Product> product);
    }
}
