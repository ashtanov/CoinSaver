using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public class StatVM
    {
        public class CatStat
        {
            public int Count { get; set; }
            public int Summ { get; set; }
            public string HistPerc { get; set; }
        }

        public string Name { get; set; }
        public int TotalSpend { get; set; }
        public int TotalPurchases { get; set; }
        public IEnumerable<KeyValuePair<PurchaseCategory, CatStat>> PurchasesByCategory { get; set; }
        public PeriodVM Period { get; set; }
    }
}
