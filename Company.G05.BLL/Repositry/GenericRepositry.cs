using Company.G05.BLL.IRepositry;
using Company.G05.DAL.Data.Contexts;
using Company.G05.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G05.BLL.Repositry
{
    public class GenericRepositry<T> : IGenericRepositry<T> where T : BaseEntity
    {
        private readonly CompanyDbContext _context;

        public GenericRepositry(CompanyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T? Get(int id)
        {
            return _context.Set<T>().Find();
        }

        public int Add(T model)
        {
            _context.Set<T>().Add(model);
            return _context.SaveChanges();
        }

        public int Update(T model)
        {
            _context.Set<T>().Update(model);
            return _context.SaveChanges();
        }

        public int Delete(T model)
        {
            _context.Set<T>().Remove(model);
            return _context.SaveChanges();
        }

    }
}
