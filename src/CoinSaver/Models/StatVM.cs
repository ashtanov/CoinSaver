using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public class StatVM
    {
        public string Name { get; set; }
        public int TotalSpend { get; set; }
        public int SpendLastMonth { get; set; }

        public int TotalPurchises { get; set; }
        public int PurchisesLastMonth { get; set; }

    }
}
