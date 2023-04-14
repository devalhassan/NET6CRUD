using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NET6CRUD.Data;
using NET6CRUD.Models;
using NET6CRUD.Models.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace NET6CRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public EmployeesController(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _dbContext.Employees.Include(x => x.Department).ToListAsync();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var departments = _dbContext.Departments.ToList();
            var model = new AddEmployeeViewModel();
            model.DepartmentsSelectList = new List<SelectListItem>();
            foreach (var department in departments)
            {
                model.DepartmentsSelectList.Add(new SelectListItem
                {
                    Text = department.DepartmentName,
                    Value = department.DepartmentID.ToString()
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel emp)
        {
            var path = Path.Combine(_environment.WebRootPath, "images", emp.Image.FileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await emp.Image.CopyToAsync(stream);
                stream.Close();
            }

            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = emp.Name,
                Email = emp.Email,
                Salary = emp.Salary,
                DateOfBirth = emp.DateOfBirth,
                IsActive = emp.IsActive,
                Gender = emp.Gender,
                Image = path,
                DepartmentID = emp.DepartmentID
            };

            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var emp = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (emp != null)
            {
                var departments = _dbContext.Departments.ToList();

                var model = new UpdateEmployeeViewModel();

                model.Id = emp.Id;
                model.Name = emp.Name;
                model.Email = emp.Email;
                model.Salary = emp.Salary;
                model.DateOfBirth = emp.DateOfBirth;
                model.IsActive = emp.IsActive;
                model.Gender = emp.Gender;
                model.ImageName = string.IsNullOrEmpty(emp.Image)? "user.png" : Path.GetFileName(emp.Image);

                model.DepartmentsSelectList = new List<SelectListItem>();
                foreach (var department in departments)
                {
                    model.DepartmentsSelectList.Add(new SelectListItem
                    {
                        Text = department.DepartmentName,
                        Value = department.DepartmentID.ToString()
                    });
                }
                model.DepartmentID = emp.DepartmentID;

                return await Task.Run(() => View("View", model));
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var emp = await _dbContext.Employees.FindAsync(model.Id);

            if (emp != null)
            {
                var path = Path.Combine(_environment.WebRootPath, "images", model.Image.FileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                    stream.Close();
                }

                emp.Name = model.Name;
                emp.Email = model.Email;
                emp.Salary = model.Salary;
                emp.DateOfBirth = model.DateOfBirth;
                emp.IsActive = model.IsActive;
                emp.Gender = model.Gender;
                emp.Image = path;
                emp.DepartmentID = model.DepartmentID;

                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var emp = await _dbContext.Employees.FindAsync(model.Id);

            if (emp != null)
            {
                _dbContext.Employees.Remove(emp);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
