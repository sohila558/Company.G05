using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G05.BLL.IRepositry
{
    public interface IUnitOfWork : IDisposable
    {
        IDepartmentRepositry DepartmentRepositry { get; }
        IEmployeeRepositry EmployeeRepositry { get; }
        int Complete();
    }
}
