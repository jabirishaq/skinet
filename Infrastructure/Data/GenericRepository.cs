using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        public StoreContext _context { get; set; }
        public GenericRepository(StoreContext context) 
        {
            _context = context;
   
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
           return await _context.Set<T>().ToListAsync();
        }

        Task<T> IGenericRepository<T>.GetEntityWithSpec(ISpecifications<T> spec)
        {
            throw new NotImplementedException();
        }

        Task<IReadOnlyCollection<T>> IGenericRepository<T>.ListAsync(ISpecifications<T> spec)
        {
            throw new NotImplementedException();
        }

        
    }
}