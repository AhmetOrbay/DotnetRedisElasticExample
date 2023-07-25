using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turkai.Model.Dtos
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public double DiscountPercentage { get; set; }
        public double Rating { get; set; }
        public long Stock { get; set; }
        public string Brand { get; set; }

        public string Category { get; set; }
        public Uri Thumbnail { get; set; }
        public Uri[] Images { get; set; }
    }
}
