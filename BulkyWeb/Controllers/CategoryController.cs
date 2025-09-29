using BulkyWeb.Data;
using Microsoft.AspNetCore.Mvc;
using BulkyWeb.Models;
using Bulky.DataAccess.Repository.IRepository;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;
        }

        public IActionResult Index()
        {
           List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }


        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
                ModelState.AddModelError("name", "DisplayOrder cannot match Name");

            if (obj.Name.ToLower() == "test")
                ModelState.AddModelError("", "Test is an invalid value");

            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category created successfully!";
                return RedirectToAction("Index");
            }

            return View(obj);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _categoryRepo.Get(c => c.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }


        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var category = _categoryRepo.Get(c => c.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }



        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _categoryRepo.Get(c => c.Id == id);
            if (obj == null) return NotFound();

            _categoryRepo.Remove(obj); 
            _categoryRepo.Save();       
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
