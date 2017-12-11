using PersonalSpendingAnalysis.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWebApp.Controllers.PSA
{
    public class BudgetController : Controller
    {
        IBudgetsService budgetService;
        public BudgetController(IBudgetsService _budgetService)
        {
            budgetService = _budgetService;
        }

        // GET: Budget
        public ActionResult Index()
        {
            var list = budgetService.GetBudgets();
            return View(list);
        }

        // GET: Budget/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Budget/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Budget/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Budget/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Budget/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Budget/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Budget/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
