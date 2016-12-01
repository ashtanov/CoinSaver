using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CoinSaver.Models;
using Microsoft.AspNetCore.Authorization;
using CoinSaver.Models.MainViewModels;

namespace CoinSaver.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<CSUser> _userManager;
        private readonly SignInManager<CSUser> _signInManager;
        private readonly RoleManager<CSRole> _roleManager;
        private readonly CoinSaverContext _dbContext;

        public HomeController(
            UserManager<CSUser> userManager,
            SignInManager<CSUser> signInManager,
            RoleManager<CSRole> roleManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = new CoinSaverContext();

        }

        [Authorize(Roles = "Administrator")] //TODO: requests GetHistoryTable with username too
        public async Task<IActionResult> UserStat(PeriodVM period, string username)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return RedirectToAction("Error");
            ViewData["Name"] = user.RealName ?? user.UserName;
            var expUser = username == null ? user : _userManager.Users.FirstOrDefault(x => x.NormalizedUserName == username.ToUpper());
            var spendings = _dbContext.GetUserPurchases(expUser);

            if (spendings.Any())
            {
                //filter by period
                if (period != null && period.IsActive)
                    spendings = spendings.WhereDateBetween(period.Start, period.End);
                else
                {
                    period.Start = spendings.Min(x => x.Date);
                    period.End = DateTime.Now;
                }


                //calc stat model
                var totalSpend = spendings.Sum(x => x.Value);
                var calcStat = spendings
                            .GroupBy(x => x.Category)
                            .ToDictionary(k => k.Key, e =>
                            {
                                var summ = e.Sum(c => c.Value);
                                return new StatVM.CatStat
                                {
                                    Count = e.Count(),
                                    Summ = summ,
                                    HistPerc = ((summ + 0.0) / totalSpend * 100).ToString().Replace(',', '.')
                                };
                            }).OrderByDescending(o => o.Value.Summ);
                return View("Stat",
                    new StatVM
                    {
                        Name = user.UserName,
                        TotalPurchases = spendings.Count(),
                        TotalSpend = spendings.Sum(x => x.Value),
                        PurchasesByCategory = calcStat,
                        Period = period
                    });
            }
            else
                return View("Stat",
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
        public async Task<IActionResult> Stat(PeriodVM period)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return RedirectToAction("Error");
            ViewData["Name"] = user.RealName ?? user.UserName;
            var spendings = _dbContext.GetUserPurchases(user);

            if (spendings.Any())
            {
                //filter by period
                if (period != null && period.IsActive)
                    spendings = spendings.WhereDateBetween(period.Start, period.End);
                else
                {
                    period.Start = spendings.Min(x => x.Date);
                    period.End = DateTime.Now;
                }


                //calc stat model
                var totalSpend = spendings.Sum(x => x.Value);
                var calcStat = spendings
                            .GroupBy(x => x.Category)
                            .ToDictionary(k => k.Key, e =>
                            {
                                var summ = e.Sum(c => c.Value);
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
                        TotalSpend = spendings.Sum(x => x.Value),
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
            var tt = _userManager.GetRolesAsync(user).Result;
            if (user == null)
                return RedirectToAction("Login", "Account", new { });
            ViewData["Name"] = user.RealName ?? user.UserName;
            var pur = _dbContext.GetUserPurchases(user).Cast<Record>().OrderByDescending(x => x.Date).Take(20).ToList();
            var sup = _dbContext.GetUserSupplies(user).Cast<Record>().OrderByDescending(x => x.Date).Take(20).ToList();

            var purB = _dbContext.GetUserPurchases(user).Sum(x => x.Value);
            var supB = _dbContext.GetUserSupplies(user).Sum(x => x.Value);
            var records = 
                pur.Union(sup)
                .OrderByDescending(x => x.Date)
                .Take(20)
                .ToList();
            var tableVm = new HistoryTableVM
            {
                ShowCategoryColumn = true,
                Record = records
            };
            return View(new IndexViewModel
            {
                Name = user.UserName,
                PurchasesTable = tableVm,
                Balance = supB - purB
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPurchase(Purchase purchase)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (purchase != null && ModelState.IsValid && user != null)
            {
                purchase.Date = DateTime.Now;
                _dbContext.Purchases.Add(
                    new CSPurchase
                    {
                        UserID = user.Id,
                        Category = purchase.Category,
                        Date = purchase.Date,
                        Name = purchase.PurchaseName,
                        Price = purchase.Value,
                        Reason = purchase.Reason
                    });
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSupply(Supply supply)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (supply != null && ModelState.IsValid && user != null)
            {
                supply.Date = DateTime.Now;
                _dbContext.Supplies.Add(
                    new CSSupply
                    {
                        Name = supply.SupplyName,
                        Value = supply.Value,
                        Date = supply.Date,
                        UserID = user.Id
                    }
                );
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetHistoryTable(HistorySettingsVM hvm)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return new StatusCodeResult(403);
            var table = _dbContext.GetUserPurchases(user);
            var tableVM = new HistoryTableVM { ShowCategoryColumn = false };

            DateTime start, end;
            if (DateTime.TryParse(hvm.StartDate, out start) && DateTime.TryParse(hvm.EndDate, out end))
                table = table.WhereDateBetween(start, end);

            PurchaseCategory currCat;
            if (Enum.TryParse(hvm.Category, out currCat))
                table = table.Where(x => x.Category == currCat);
            else if (hvm.Category != "All")
                return new StatusCodeResult(500);
            else
                tableVM.ShowCategoryColumn = true;
            tableVM.Record = table.Cast<Record>().ToList();

            return PartialView("HistoryTable", tableVM);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
