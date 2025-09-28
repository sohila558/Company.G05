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
        private readonly IDepartmentRepositry _departmentRepositry;
        private readonly IMapper _mapper;

        // Ask CLR to Create Object From DepartmentRepositry
        public DepartmentController(IDepartmentRepositry departmentRepositry, IMapper mapper)
        {
            _departmentRepositry = departmentRepositry;
            _mapper = mapper;
        }

        [HttpGet] // GET: /Department/Index
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Department> department;
            if (string.IsNullOrEmpty(SearchInput))
            {
                department = _departmentRepositry.GetAll();
            }
            else
            {
                department = _departmentRepositry.GetByName(SearchInput);
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
            if (id is null) return BadRequest("Invalid Id !"); // 400

            var department = _departmentRepositry.Get(id.Value);

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
            var department = _mapper.Map<Department>(model);

            department.Id = id;

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
