using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turkai.Model.ExtensionModel.DummpyDataModel
{
    public class DummpyResponse
    {
        [JsonProperty("products")]
        public ProductDummpy[] Products { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("skip")]
        public long Skip { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }
    }
}
