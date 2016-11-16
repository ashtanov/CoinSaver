using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models.MainViewModels
{
    public class PurchasesTableVM
    {
        public IList<Purchase> Purchases { get; set; }
        public bool ShowCategoryColumn { get; set; }
    }
}
