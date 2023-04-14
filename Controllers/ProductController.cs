using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NET6CRUD.Models.Domain;
using NET6CRUD.Models;

namespace NET6CRUD.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public ProductController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel product)
        {
            string path = string.Empty;
            FileStream stream = null;

            foreach (var attatchment in product.Attatchments)
            {
                path = Path.Combine(_environment.WebRootPath, "files", attatchment.FileName);
                stream = new FileStream(path, FileMode.Create);
                await attatchment.CopyToAsync(stream);
                stream.Close();
            }

            return RedirectToAction("Add");
        }
    }
}
