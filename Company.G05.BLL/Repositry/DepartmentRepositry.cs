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
    public class DepartmentRepositry : GenericRepositry<Department>, IDepartmentRepositry
    {
        private readonly CompanyDbContext _context;
        public DepartmentRepositry(CompanyDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Department>> GetByNameAsync(string name)
        {
            return await _context.Departments.Where(E => E.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
