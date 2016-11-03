using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CoinSaver.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoinSaver.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<CSUser> _userManager;
        private readonly SignInManager<CSUser> _signInManager;
        private readonly CoinSaverContext _dbContext;

        public HomeController(
            UserManager<CSUser> userManager,
            SignInManager<CSUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = new CoinSaverContext();
            MigrateFromOldDb();

        }

        bool processed = false;
        public void MigrateFromOldDb()
        {
            if (!processed)
            {
                var oldbase = new FileDB();
                foreach (var i in oldbase._db)
                {
                    CSUser usr = _userManager.Users.Where(x => x.NormalizedUserName == i.Key.ToUpper())?.FirstOrDefault();
                    if (usr != null)
                    {
                        var t = _dbContext.GetUserSpendings(usr);
                        foreach (var oldItem in i.Value)
                        {
                            if (!t.Any(x => x.Date == oldItem.Date && x.PurchaseName == oldItem.PurchaseName))
                            {
                                _dbContext.Add(
                                    new CSPurchase
                                    {
                                        Date = oldItem.Date,
                                        Category = oldItem.Category,
                                        Name = oldItem.PurchaseName,
                                        Price = oldItem.Price,
                                        Reason = PurchaseReason.Need,
                                        UserID = usr.Id
                                    });
                            }
                        }

                    }

                }
                _dbContext.SaveChanges();
            }
            processed = true;
        }

        [Authorize]
        public async Task<IActionResult> Stat(PeriodVM period)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return RedirectToAction("Error");
            ViewData["Name"] = user.RealName ?? user.UserName;
            var spendings = _dbContext.GetUserSpendings(user);

            if (spendings.Any())
            {
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
                                return new StatVM.CatStat
                                {
                                    Count = e.Count(),
                                    Summ = summ,
                                    HistPerc = ((summ + 0.0) / totalSpend * 100).ToString().Replace(',', '.')
                                };
                            }).OrderByDescending(o => o.Value.Summ);
                return View(
                    new StatVM
                    {
                        Name = user.UserName,
                        TotalPurchases = spendings.Count(),
                        TotalSpend = spendings.Sum(x => x.Price),
                        PurchasesByCategory = calcStat,
                        Period = period
                    });
            }
            else
                return View(
                    new StatVM
                    {
                        Name = user.UserName,
                        TotalPurchases = 0,
                        TotalSpend = 0,
                        PurchasesByCategory = null,
                        Period = period
                    });
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { });
            ViewData["Name"] = user.RealName ?? user.UserName;
            return View(new IndexViewModel
            {
                Name = user.UserName,
                Purchases = _dbContext.GetUserSpendings(user)
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Purchase spending)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (spending != null && ModelState.IsValid)
            {
                spending.Date = DateTime.Now;
                _dbContext.Purchases.Add(
                    new CSPurchase
                    {
                        UserID = user.Id,
                        Category = spending.Category,
                        Date = spending.Date,
                        Name = spending.PurchaseName,
                        Price = spending.Price,
                        Reason = spending.Reason
                    });
                await _dbContext.SaveChangesAsync();

            }
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
