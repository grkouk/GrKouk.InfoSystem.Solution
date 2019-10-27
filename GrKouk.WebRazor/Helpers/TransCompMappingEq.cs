using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.WebRazor.Helpers
{
    public class TransCompMappingEq : IEqualityComparer<TransactorCompanyMapping>
    {
        public bool Equals(TransactorCompanyMapping x, TransactorCompanyMapping y)
        {
            if (x.CompanyId==y.CompanyId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(TransactorCompanyMapping obj)
        {
            throw new NotImplementedException();
        }
    }
}
