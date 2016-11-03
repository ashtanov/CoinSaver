using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace CoinSaver.Models
{
    public class Purchase
    {
        [Range(1, 99999999, ErrorMessage = "Цена должа быть больше 1")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Введите название покупки")]
        public string PurchaseName { get; set; }

        public PurchaseReason Reason { get; set; }

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
                Reason = res[2].IsInt() ? (PurchaseReason)Enum.Parse(typeof(PurchaseReason), res[2]) : PurchaseReason.Need,
                Date = DateTime.Parse(res[3], DateTimeFormatInfo.InvariantInfo),
                Category = (PurchaseCategory)Enum.Parse(typeof(PurchaseCategory), res[4])
            };
        }
    }
}
