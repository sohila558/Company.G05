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
    public class DepartmentRepositry : GenericRepositry<Department>, IDepartmentRepositry
    {
        public DepartmentRepositry(CompanyDbContext context) : base(context)
        {
             
        }
    }
}
