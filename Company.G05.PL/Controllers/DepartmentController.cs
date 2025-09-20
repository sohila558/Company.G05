using Company.G05.BLL.IRepositry;
using Company.G05.BLL.Repositry;
using Company.G05.DAL.Models;
using Company.G05.PL.DTOs;
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDTO  model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };

                var Count = _departmentRepositry.Add(department);

                if(Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }
    }
}
