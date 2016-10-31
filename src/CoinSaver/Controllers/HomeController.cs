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

        public IActionResult Stat(string id, Models.PeriodVM period)
        {
            ViewData["Name"] = id;
            if (id == null)
                return RedirectToAction("Error");
            var spendings = _db.GetSpendings(id);

            //filter by period
            if (period != null && period.IsActive)
            {
                DateTime startFormat = period.Start.Date;
                DateTime endFormat = period.End.Date.AddDays(1).AddSeconds(-1);
                spendings = spendings.Where(x => x.Date >= startFormat && x.Date <= endFormat);
            }
            else
            {
                period.Start = spendings.Min(x => x.Date);
                period.End = DateTime.Now;
            }

            //calc stat model
            var totalSpend = spendings.Sum(x => x.Price);
            var calcStat = spendings
                        .GroupBy(x => x.Category)
                        .ToDictionary(k => k.Key, e =>
                        {
                            var summ = e.Sum(c => c.Price);
                            return new Models.StatVM.CatStat
                            {
                                Count = e.Count(),
                                Summ = summ,
                                HistPerc = ((summ + 0.0) / totalSpend * 100).ToString().Replace(',', '.')
                            };
                        }).OrderByDescending(o => o.Value.Summ);
            return View(
                new Models.StatVM
                {
                    Name = id,
                    TotalPurchases = spendings.Count(),
                    TotalSpend = spendings.Sum(x => x.Price),
                    PurchasesByCategory = calcStat,
                    Period = period
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Models.PurNameVM spending)
        {
            if (spending.Pur != null && ModelState.IsValid)
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
