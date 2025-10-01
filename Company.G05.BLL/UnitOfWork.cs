using Company.G05.BLL.IRepositry;
using Company.G05.BLL.Repositry;
using Company.G05.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G05.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _context;

        public IDepartmentRepositry DepartmentRepositry { get; }

        public IEmployeeRepositry EmployeeRepositry { get; }

        public UnitOfWork(CompanyDbContext context)
        {
            _context = context;
            DepartmentRepositry = new DepartmentRepositry(_context);
            EmployeeRepositry = new EmployeeRepositry(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
