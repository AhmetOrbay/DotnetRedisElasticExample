using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turkai.Model.Entity;
using Turkai.Model.ExtensionModel.Enums;

namespace Turkai.Service.Interfaces
{
    public interface IRabbitMQService
    {
        Task WriteRabbitMq(string product, Routing Routing);
        Task ReadRabbiMQ(RabbitMQEnum key);
    }
}
