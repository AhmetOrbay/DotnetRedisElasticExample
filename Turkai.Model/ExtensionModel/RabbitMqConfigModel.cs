using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turkai.Model.ExtensionModel
{
    public class RabbitMqConfigModel
    {
        public required string HostName { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string VirtualHost { get; set; }
    }
}
