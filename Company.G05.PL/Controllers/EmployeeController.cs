using AutoMapper;
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
        private readonly IMapper _mapper;

        public EmployeeController(
            IEmployeeRepositry employeeRepositry,
            IDepartmentRepositry departmentRepositry,
            IMapper mapper)
        {
            _employeeRepositry = employeeRepositry;
            _departmentRepositry = departmentRepositry;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = _employeeRepositry.GetAll();
            }
            else
            {
                employees = _employeeRepositry.GetByName(SearchInput);
            }


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
                try
                {
                    // Manual Mapping
                    //var employee = new Employee()
                    //{
                    //    Name = model.Name,
                    //    Age = model.Age,
                    //    Email = model.Email,
                    //    Address = model.Address,
                    //    Phone = model.Phone,
                    //    Salary = model.Salary,
                    //    IsActive = model.IsActive,
                    //    IsDeleted = model.IsDeleted,
                    //    CreateAt = model.CreateAt,
                    //    HiringDate = model.HiringDate,
                    //    DepartmentId = model.DepartmentId
                    //};

                    var employee = _mapper.Map<Employee>(model);
                    var Count = _employeeRepositry.Add(employee);

                    if (Count > 0)
                    {
                        //TempData["Message"] = "Employee is Created";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
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

            var employeeDto = _mapper.Map<EmployeeDTO>(employee); 

            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest();
                var count = _employeeRepositry.Update(model);

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
            //if (id is null) return BadRequest("Invalid Id !"); // 400

            //var employee = _employeeRepositry.Get(id.Value);

            //if (employee is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, EmployeeDTO model)
        {
            var employee = _mapper.Map<Employee>(model);

            employee.Id = id;

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
