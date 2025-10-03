using AutoMapper;
using Company.G05.BLL;
using Company.G05.BLL.IRepositry;
using Company.G05.BLL.Repositry;
using Company.G05.DAL.Models;
using Company.G05.PL.DTOs;
using Company.G05.PL.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Company.G05.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepositry _employeeRepositry;
        //private readonly IDepartmentRepositry _departmentRepositry;
        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepositry employeeRepositry,
            //IDepartmentRepositry departmentRepositry,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            //_employeeRepositry = employeeRepositry;
            //_departmentRepositry = departmentRepositry;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = _unitOfWork.EmployeeRepositry.GetAll();
            }
            else
            {
                employees = _unitOfWork.EmployeeRepositry.GetByName(SearchInput);
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
            var departments = _unitOfWork.DepartmentRepositry.GetAll();
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
                    if(model.Image is not null)
                    {
                        model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                    }

                    var employee = _mapper.Map<Employee>(model);
                    _unitOfWork.EmployeeRepositry.Add(employee);
                    var Count = _unitOfWork.Complete();

                    if (Count > 0)
                    {
                        TempData["Message"] = "Employee is Created";
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

            var employee = _unitOfWork.EmployeeRepositry.Get(id.Value);

            if (employee is null) return NotFound(new { statusCode = 404, message = $"Employee with Id: {id} Not Found" });

            return View(viewName, employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var departments = _unitOfWork.DepartmentRepositry.GetAll();

            ViewData["departments"] = departments;

            if (id is null) return BadRequest("Invalid Id !"); // 400

            var employee = _unitOfWork.EmployeeRepositry.Get(id.Value);

            if (employee is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            var employeeDto = _mapper.Map<EmployeeDTO>(employee); 

            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeDTO model)
        {
            if (ModelState.IsValid)
            {

                if(model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }

                if(model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;

                if (id != employee.Id) return BadRequest();

                _unitOfWork.EmployeeRepositry.Update(employee);
                var Count = _unitOfWork.Complete();

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
                _unitOfWork.EmployeeRepositry.Delete(employee);
                var Count = _unitOfWork.Complete();

                if (Count > 0)
                {
                    if(model.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(model.ImageName, "Images");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
