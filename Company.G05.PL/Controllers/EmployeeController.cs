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
        private readonly IDepartmentRepositry _departmentRepositry;

        public EmployeeController(IEmployeeRepositry employeeRepositry, IDepartmentRepositry departmentRepositry)
        {
            _employeeRepositry = employeeRepositry;
            _departmentRepositry = departmentRepositry;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _employeeRepositry.GetAll();

            // Dictionary :
            // 1. ViewData : Transfer Extra Information From Controller (Action) To View

            //ViewData["Message"] = "Hello From ViewData";

            // 2. ViewBag  : Transfer Extra Information From Controller (Action) To View

            //ViewBag.Message = "Hello From ViewBag";

            // 3. TempData

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentRepositry.GetAll();
            ViewData["departments"] = departments;
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
                    HiringDate = model.HiringDate,
                    DepartmentId = model.DepartmentId
                };

                var Count = _employeeRepositry.Add(employee);

                if (Count > 0)
                {
                    //TempData["Message"] = "Employee is Created";
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
            var departments = _departmentRepositry.GetAll();

            ViewData["departments"] = departments;

            if (id is null) return BadRequest("Invalid Id !"); // 400

            var employee = _employeeRepositry.Get(id.Value);

            if (employee is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            var employeeDto = new EmployeeDTO()
            {
                Name = employee.Name,
                Age = employee.Age,
                Email = employee.Email,
                Address = employee.Address,
                Phone = employee.Phone,
                Salary = employee.Salary,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted,
                CreateAt = employee.CreateAt,
                HiringDate = employee.HiringDate
            };

            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee model)
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
                DepartmentId = model.DepartmentId,
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
            if (id is null) return BadRequest("Invalid Id !"); // 400

            var employee = _employeeRepositry.Get(id.Value);

            if (employee is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

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
