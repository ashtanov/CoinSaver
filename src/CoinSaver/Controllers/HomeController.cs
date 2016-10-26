﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoinSaver.Controllers
{
    public class HomeController : Controller
    {
        private readonly Models.IDataLayer _db;
        public HomeController(Models.IDataLayer db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult EnterName(Models.EnteredName name)
        {
            if (name != null)
                return RedirectToAction("Index", new { id = name.Name });
            else
                return RedirectToAction("Error");
        }

        public IActionResult Index(string id)
        {
            if(id == null)
                return View();
            return View(new Models.IndexViewModel { Name = id, Purchases = _db.GetSpendings(id) });
        }

        [HttpPost]
        public async Task<IActionResult> Index(Models.PurName spending)
        {
            if (spending.Pur != null)
            {
                spending.Pur.date = DateTime.Now;
                await _db.SaveSpendingAsync(spending.Name, spending.Pur);
            }
            return RedirectToAction("Index", new { id = spending.Name });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
