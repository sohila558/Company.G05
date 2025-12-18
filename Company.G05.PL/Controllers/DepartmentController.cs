using AutoMapper;
using Company.G05.BLL.IRepositry;
using Company.G05.BLL.Repositry;
using Company.G05.DAL.Models;
using Company.G05.PL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.G05.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // Ask CLR to Create Object From DepartmentRepositry
        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet] // GET: /Department/Index
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Department> department;
            if (string.IsNullOrEmpty(SearchInput))
            {
                department = await _unitOfWork.DepartmentRepositry.GetAllAsync();
            }
            else
            {
                department = await _unitOfWork.DepartmentRepositry.GetByNameAsync(SearchInput);
            }

            return View(department);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDTO  model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var department = _mapper.Map<Department>(model);

                await _unitOfWork.DepartmentRepositry.AddAsync(department);
                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromRoute]int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id !"); // 400

            var department = await _unitOfWork.DepartmentRepositry.GetAsync(id.Value);

            if (department is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            return View(viewName, department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id !"); // 400

            var department = await _unitOfWork.DepartmentRepositry.GetAsync(id.Value);

            if (department is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            var departmentDto = _mapper.Map<CreateDepartmentDTO>(department);

            return View(departmentDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateDepartmentDTO model)
        {

            if (ModelState.IsValid)
            {

                var department = _mapper.Map<Department>(model);
                department.Id = id;

                _unitOfWork.DepartmentRepositry.Update(department);
                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id !"); // 400

            //var department = _departmentRepositry.Get(id.Value);

            //if (department is null) return NotFound(new { statusCode = 404, message = $"Department with Id: {id} Not Found" });

            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateDepartmentDTO model)
        {
            var department = _mapper.Map<Department>(model);

            department.Id = id;

            if (ModelState.IsValid)
            {
                _unitOfWork.DepartmentRepositry.Delete(department);
                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

    }
}
