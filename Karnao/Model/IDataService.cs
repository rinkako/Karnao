using System.Threading.Tasks;

namespace Karnao.Model
{
    public interface IDataService
    {
        Task<DataItem> GetData();
    }
}