using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Turkai.Model.Entity;
using Turkai.Model.ExtensionModel.ElasticSearch;

namespace Turkai.Service.Interfaces
{
    public interface IElasicSearchService
    {
        Task ImportElasticProduct(List<ElasticImportModel> data);
        Task<List<T>> GetElasticSearchData<T>(ElasticSearchIndex Index);
    }
}
