
using Turkai.Model.ExtensionModel.DummpyDataModel;

namespace Turkai.Service.Interfaces
{
    public interface IDummpyDataService
    {
        Task<string> GetDummpyData();
    }
}
