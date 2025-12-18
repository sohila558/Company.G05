using Company.G05.BLL.IRepositry;
using Company.G05.DAL.Data.Contexts;
using Company.G05.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G05.BLL.Repositry
{
    public class GenericRepositry<TEntity> : IGenericRepositry<TEntity> where TEntity : BaseEntity
    {
        private readonly CompanyDbContext _context;

        public GenericRepositry(CompanyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if(typeof(TEntity) == typeof(Employee))
            {
                return (IEnumerable<TEntity>) await _context.Employees.Include(E => E.Department).ToListAsync();
            }
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetAsync(int id)
        {
            if (typeof(TEntity) == typeof(Employee))
            {
                return await _context.Employees.Include(E => E.Department).FirstOrDefaultAsync(E => E.Id == id) as TEntity;
            }
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity model)
        {
            await _context.Set<TEntity>().AddAsync(model);
        }

        public void Update(TEntity model)
        {
            _context.Set<TEntity>().Update(model);
        }

        public void Delete(TEntity model)
        {
            _context.Set<TEntity>().Remove(model);
        }

    }
}
