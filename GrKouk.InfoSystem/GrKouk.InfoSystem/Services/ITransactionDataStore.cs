using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Models;

namespace GrKouk.InfoSystem.Services
{
   public interface ITransactionDataStore<T1, T2, T3> : IDataStore<T1, T2, T3>
   {
       Task<SearchListItem> GetCategoryIdOfTransactorsLastTransactionAsync(int transactorId);
       Task<SearchListItem> GetCostCentreIdOfTransactorsLastTransactionAsync(int transactorId);
       Task<SearchListItem> GetRevenueCentreIdOfTransactorsLastTransactionAsync(int transactorId);
   }
}
