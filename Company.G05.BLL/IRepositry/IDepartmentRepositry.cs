using Company.G05.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G05.BLL.IRepositry
{
    public interface IDepartmentRepositry : IGenericRepositry<Department>
    {
        Task<List<Department>> GetByNameAsync(string name);
    }
}
