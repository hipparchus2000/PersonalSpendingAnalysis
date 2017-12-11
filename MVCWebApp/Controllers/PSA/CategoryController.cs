using IRepositories.Interfaces;
using IServices.Interfaces;
using PersonalSpendingAnalysis.Models;
using PersonalSpendingAnalysis.Repo;
using Services.Services;
using System;
using System.Linq;
using System.Web.Mvc;
using Unity;

namespace MVCWebApp.Controllers.PSA
{
    public class CategoryController : Controller
    {
        static IUnityContainer container = new UnityContainer();
        ICategoryService categoryService;

        public CategoryController()
        {
            container.RegisterType<IPersonalSpendingAnalysisRepo, PersonalSpendingAnalysisRepo>();
            container.RegisterType<ICategoryService, CategoryService>();
            categoryService = container.Resolve<CategoryService>();
        }

        // GET: Category
        public ActionResult Index()
        {
            var categories = categoryService.GetCategories();
            return View(categories);
        }

        // GET: Category/Details/5
        public ActionResult Details(Guid id)
        {
            var categories = categoryService.GetCategories();
            var category = categories.Single(x => x.Id == id);
            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create(CategoryModel category)
        {
            categoryService.AddNewCategory(category);
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var category = new CategoryModel
            {
                Id = Guid.NewGuid(),
                Name = collection["Name"],
                SearchString = collection["SearchString"]
            };

            try
            {
                categoryService.AddNewCategory(category);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //// GET: Category/Edit/5
        //public ActionResult Edit(Guid id)
        //{
        //    return View();
        //}

        //// POST: Category/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Category/Delete/5
        public ActionResult Delete(Guid id)
        {
            var categories = categoryService.GetCategories();
            var category = categories.Single(x => x.Id == id);
            categoryService.RemoveCategory(category);
            return View();
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            try
            {
                var categories = categoryService.GetCategories();
                var category = categories.Single(x => x.Id == id);
                categoryService.RemoveCategory(category);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
