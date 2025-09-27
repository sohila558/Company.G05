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

        public IEnumerable<TEntity> GetAll()
        {
            if(typeof(TEntity) == typeof(Employee))
            {
                return (IEnumerable<TEntity>) _context.Employees.Include(E => E.Department).ToList();
            }
            return _context.Set<TEntity>().ToList();
        }

        public TEntity? Get(int id)
        {
            if (typeof(TEntity) == typeof(Employee))
            {
                return  _context.Employees.Include(E => E.Department).FirstOrDefault(E => E.Id == id) as TEntity;
            }
            return _context.Set<TEntity>().Find(id);
        }

        public int Add(TEntity model)
        {
            _context.Set<TEntity>().Add(model);
            return _context.SaveChanges();
        }

        public int Update(TEntity model)
        {
            _context.Set<TEntity>().Update(model);
            return _context.SaveChanges();
        }

        public int Delete(TEntity model)
        {
            _context.Set<TEntity>().Remove(model);
            return _context.SaveChanges();
        }

    }
}
