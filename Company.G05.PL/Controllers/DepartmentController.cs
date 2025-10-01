using AutoMapper;
using Company.G05.BLL.IRepositry;
using Company.G05.BLL.Repositry;
using Company.G05.DAL.Models;
using Company.G05.PL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Company.G05.PL.Controllers
{
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepositry _departmentRepositry;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // Ask CLR to Create Object From DepartmentRepositry
        public DepartmentController(/*IDepartmentRepositry departmentRepositry*/ IUnitOfWork unitOfWork, IMapper mapper)
        {
            //_departmentRepositry = departmentRepositry;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet] // GET: /Department/Index
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Department> department;
            if (string.IsNullOrEmpty(SearchInput))
            {
                department = _unitOfWork.DepartmentRepositry.GetAll();
            }
            else
            {
                department = _unitOfWork.DepartmentRepositry.GetByName(SearchInput);
            }

            return View(department);
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
                var department = _mapper.Map<Department>(model);

                _unitOfWork.DepartmentRepositry.Add(department);
                var Count = _unitOfWork.Complete();

                if (Count > 0)
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

            var department = _unitOfWork.DepartmentRepositry.Get(id.Value);

            if (department is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            return View(viewName, department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id !"); // 400

            var department = _unitOfWork.DepartmentRepositry.Get(id.Value);

            if (department is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            var departmentDto = _mapper.Map<CreateDepartmentDTO>(department);

            return View(departmentDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateDepartmentDTO model)
        {

            if (ModelState.IsValid)
            {

                var department = _mapper.Map<Department>(model);
                department.Id = id;

                _unitOfWork.DepartmentRepositry.Update(department);
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

            //var department = _departmentRepositry.Get(id.Value);

            //if (department is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, CreateDepartmentDTO model)
        {
            var department = _mapper.Map<Department>(model);

            department.Id = id;

            if (ModelState.IsValid)
            {
                _unitOfWork.DepartmentRepositry.Delete(department);
                var Count = _unitOfWork.Complete();

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

    }
}
