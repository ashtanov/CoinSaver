using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public class Purchase
    {
        [Range(1, 99999999)]
        public int Price { get; set; }
        public string PurchaseName { get; set; }
        private string _reason;
        public string Reason {
            get
            {
                return _reason;
            }
            set
            {
                _reason = value?.Replace("\n", " ")?.Replace("\t", " ")?.Replace("@@@", " ");
            }
        }
        public DateTime Date { get; set; }

        public PurchaseCategory Category { get; set; }

        public override string ToString()
        {
            return $"{PurchaseName}\t{Price}\t{Reason}\t{Date}\t{Category}";
        }

        public static Purchase Parse(string purchase)
        {
            var res = purchase.Split('\t');
            return new Purchase
            {
                Price = int.Parse(res[1]),
                PurchaseName = res[0],
                Reason = res[2],
                Date = DateTime.Parse(res[3]),
                Category = (PurchaseCategory)Enum.Parse(typeof(PurchaseCategory),res[4])
            };
        }

        public bool IsValid => Price > 0 && !string.IsNullOrWhiteSpace(PurchaseName);
    }
}
