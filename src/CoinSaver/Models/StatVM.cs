using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public class StatVM
    {
        public class CountAndSumm
        {
            public int Count { get; set; }
            public int Summ { get; set; }
        }

        public string Name { get; set; }
        public int TotalSpend { get; set; }
        public int TotalPurchases { get; set; }
        public Dictionary<PurchaseCategory, CountAndSumm> PurchasesByCategory { get; set; }
        public List<KeyValuePair<PurchaseCategory, string>> HistogrammPercentage { get; set; }
        public PeriodVM Period { get; set; }
    }
}
