using Company.G05.BLL.IRepositry;
using Company.G05.BLL.Repositry;
using Company.G05.DAL.Models;
using Company.G05.PL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Company.G05.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepositry _employeeRepositry;
        public EmployeeController(IEmployeeRepositry employeeRepositry)
        {
            _employeeRepositry = employeeRepositry;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _employeeRepositry.GetAll();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var employee = new Employee()
                {
                    Name = model.Name,
                    Age = model.Age,
                    Email = model.Email,
                    Address = model.Address,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate
                };

                var Count = _employeeRepositry.Add(employee);

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Details([FromRoute] int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id !");

            var employee = _employeeRepositry.Get(id.Value);

            if (employee is null) return NotFound(new { statusCode = 404, message = $"Employee with Id: {id} Not Found" });

            return View(viewName, employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, Employee model)
        {
            var employee = new Employee()
            {
                Id = id.Value,
                Name = model.Name,
                Age = model.Age,
                Email = model.Email,
                Address = model.Address,
                Phone = model.Phone,
                Salary = model.Salary,
                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                CreateAt = model.CreateAt,
                HiringDate = model.HiringDate
            };

            if (ModelState.IsValid)
            {
                var count = _employeeRepositry.Update(employee);

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, EmployeeDTO model)
        {
            var employee = new Employee()
            {
                Id = id,
                Name = model.Name,
                Age = model.Age,
                Email = model.Email,
                Address = model.Address,
                Phone = model.Phone,
                Salary = model.Salary,
                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                CreateAt = model.CreateAt,
                HiringDate = model.HiringDate
            };

            if (ModelState.IsValid)
            {
                var Count = _employeeRepositry.Delete(employee);

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
