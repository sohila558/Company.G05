using Company.G05.BLL.IRepositry;
using Company.G05.BLL.Repositry;
using Microsoft.AspNetCore.Mvc;

namespace Company.G05.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepositry _departmentRepositry;

        // Ask CLR to Create Object From DepartmentRepositry
        public DepartmentController(IDepartmentRepositry departmentRepositry)
        {
            _departmentRepositry = departmentRepositry;
        }

        [HttpGet] // GET: /Department/Index
        public IActionResult Index()
        {
            var departments = _departmentRepositry.GetAll();

            return View(departments);
        }
    }
}
