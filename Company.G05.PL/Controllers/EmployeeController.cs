using AutoMapper;
using Company.G05.BLL;
using Company.G05.BLL.IRepositry;
using Company.G05.BLL.Repositry;
using Company.G05.DAL.Models;
using Company.G05.PL.DTOs;
using Company.G05.PL.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.G05.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepositry.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepositry.GetByNameAsync(SearchInput);
            }

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepositry.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDTO model)
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
                    await _unitOfWork.EmployeeRepositry.AddAsync(employee);
                    var Count = await _unitOfWork.CompleteAsync();

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

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromRoute] int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id !");

            var employee = await _unitOfWork.EmployeeRepositry.GetAsync(id.Value);

            if (employee is null) return NotFound(new { statusCode = 404, message = $"Employee with Id: {id} Not Found" });

            var dto = _mapper.Map<Employee>(employee);

            return View(viewName, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string viewName = "Edit")
        {
            if (id is null) return BadRequest("Invalid Id !"); // 400
            
            var employee = await _unitOfWork.EmployeeRepositry.GetAsync(id.Value);

            var departments = await _unitOfWork.DepartmentRepositry.GetAllAsync();
           
            ViewData["departments"] = departments;

            if (employee is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            var employeeDto = _mapper.Map<EmployeeDTO>(employee); 

            return View(viewName, employeeDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeDTO model, string viewName = "Edit")
        {
            if (ModelState.IsValid)
            {
                if (model.ImageName is not null && model.Image is not null)
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
                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(viewName, model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id !"); // 400

            //var employee = _employeeRepositry.Get(id.Value);

            //if (employee is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeDTO model)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;

                _unitOfWork.EmployeeRepositry.Delete(employee);

                var Count = await _unitOfWork.CompleteAsync();

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
