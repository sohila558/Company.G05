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
    public class EmployeeRepositry : GenericRepositry<Employee>, IEmployeeRepositry
    {
        public EmployeeRepositry(CompanyDbContext context) : base(context) // Ask CLR to Create Object from CompanyDbContext
        {
            
        }
    }
}
