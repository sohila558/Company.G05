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

        [HttpGet]
        public IActionResult Details([FromRoute]int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id !"); // 400

            var department = _departmentRepositry.Get(id.Value);

            if (department is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            return View(viewName, department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id !"); // 400

            //var department = _departmentRepositry.Get(id.Value);

            //if (department is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, CreateDepartmentDTO model)
        {
            var department = new Department()
            {
                Id = id.Value,
                Name = model.Name,
                Code = model.Code,
                CreateAt = model.CreateAt
            };

            if (ModelState.IsValid)
            {
                var Count = _departmentRepositry.Update(department);

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id !"); // 400

            //var department = _departmentRepositry.Get(id.Value);

            //if (department is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, CreateDepartmentDTO model)
        {
            var department = new Department()
            {
                Id = id,
                Name = model.Name,
                Code = model.Code,
                CreateAt = model.CreateAt
            };

            if (ModelState.IsValid)
            {
                var Count = _departmentRepositry.Delete(department);

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

    }
}
