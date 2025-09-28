using BulkyWeb.Data;
using Microsoft.AspNetCore.Mvc;
using BulkyWeb.Models;
using Bulky.DataAccess.Repository.IRepository;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        // LIST ALL CATEGORIES
        public IActionResult Index()
        {
            var categories = _categoryRepo.GetAll().ToList();
            return View(categories);
        }

        // SHOW CREATE FORM
        public IActionResult Create() => View();

        // HANDLE CREATE FORM POST
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
                ModelState.AddModelError("name", "DisplayOrder cannot match Name");

            if (obj.Name.ToLower() == "test")
                ModelState.AddModelError("", "Test is an invalid value");

            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj); // add to DbContext
                _categoryRepo.Save();   // commit to database
                TempData["success"] = "Category created successfully!";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        // SHOW EDIT FORM
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var category = _categoryRepo.Get(c => c.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

        // HANDLE EDIT POST
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj); // mark entity as updated
                _categoryRepo.Save();       // commit changes
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        // SHOW DELETE CONFIRMATION
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var category = _categoryRepo.Get(c => c.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

        // HANDLE DELETE POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _categoryRepo.Get(c => c.Id == id);
            if (obj == null) return NotFound();

            _categoryRepo.Delete(obj); // remove entity
            _categoryRepo.Save();       // commit changes
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
