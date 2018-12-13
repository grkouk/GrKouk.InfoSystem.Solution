using System.Collections.Generic;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Models;

namespace GrKouk.InfoSystem.Services
{
    public interface IAutoCompleteDataSource<T>
    {
        Task<IEnumerable<SearchListItem>> GetSearchListItemsAsync();
        Task<IList<SearchListItem>> GetSearchListItemsLightAsync();
    }
}
