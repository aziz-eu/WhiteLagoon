using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interface;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public void Add(Villa Entity)
        {
            _db.Add(Entity);
            
        }

        public Villa Get(Expression<Func<Villa, bool>> filter, string? includeProperties = null)
        {
            IQueryable<Villa> query = _db.Set<Villa>();
            if(filter != null) {
            query  = query.Where(filter);   
            }
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<Villa> Getll(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Villa> query = _db.Set<Villa>();

            if(filter != null)
            {
                query = query.Where(filter);
            }
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] {',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query.Include(property);
                }
            }
            return query.ToList();
        }

        public void Remove(Villa Entity)
        {
            _db.Remove(Entity);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Villa Entity)
        {
            _db.Update(Entity);
        }
    }
}
