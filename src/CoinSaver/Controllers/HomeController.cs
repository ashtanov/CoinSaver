using System;
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
        public IActionResult EnterName(Models.EnteredNameVM name)
        {
            if (name != null)
                return RedirectToAction("Index", new { id = name.Name });
            return RedirectToAction("Error");
        }

        public IActionResult Stat(string id)
        {
            ViewData["Name"] = id;
            if (id == null)
                return RedirectToAction("Error");
            var spendings = _db.GetSpendings(id);

            var totalSpend = spendings.Sum(x => x.Price);
            var calcStat = spendings
                        .GroupBy(x => x.Category)
                        .ToDictionary(k => k.Key, e => new Models.StatVM.CountAndSumm
                        {
                            Count = e.Count(),
                            Summ = e.Sum(c => c.Price)
                        });
            var pbp = new List<KeyValuePair<Models.PurchaseCategory, string>>
                        (calcStat
                            .Select(x => new KeyValuePair<Models.PurchaseCategory, string>(x.Key, ((x.Value.Summ + 0.0) / totalSpend * 100).ToString().Replace(',','.'))));
            return View(
                new Models.StatVM
                {
                    Name = id,
                    TotalPurchases = spendings.Count(),
                    TotalSpend = spendings.Sum(x => x.Price),
                    PurchasesByCategory = calcStat,
                    ProgressBarPercentage = pbp
                });
        }

        public IActionResult Index(string id)
        {
            ViewData["Name"] = id;
            if (id == null)
                return View();
            return View(new Models.IndexViewModel { Name = id, Purchases = _db.GetSpendings(id) });
        }

        [HttpPost]
        public async Task<IActionResult> Index(Models.PurNameVM spending)
        {
            if (spending.Pur != null && spending.Pur.IsValid)
            {
                spending.Pur.Date = DateTime.Now;
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
