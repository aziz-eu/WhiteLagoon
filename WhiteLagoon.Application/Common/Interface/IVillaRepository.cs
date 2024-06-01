using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interface
{
    public interface IVillaRepository
    {
        IEnumerable<Villa> Getll(Expression<Func<Villa ,bool>>? filter = null, string? includeProperties = null);
        IEnumerable<Villa> Get(Expression<Func<Villa, bool>> filter, string? includeProperties = null);
        void Add(Villa Entity);
        void Update(Villa Entity);
        void Remove(Villa Entity);
        void Save();
    }
}
