using Company.G05.BLL.IRepositry;
using Company.G05.DAL.Data.Contexts;
using Company.G05.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G05.BLL.Repositry
{
    public class EmployeeRepositry : IEmployeeRepositry
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepositry(CompanyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public Employee? Get(int id)
        {
            return _context.Employees.Find();
        }

        public int Add(Employee model)
        {
            _context.Employees.Add(model);
            return _context.SaveChanges();
        }

        public int Update(Employee model)
        {
            _context.Employees.Update(model);
            return _context.SaveChanges();
        }

        public int Delete(Employee model)
        {
            _context.Employees.Remove(model);
            return _context.SaveChanges();
        }

    }
}
