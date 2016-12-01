using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace CoinSaver.Models
{
    public class Purchase : Record
    {
        [Range(1, int.MaxValue, ErrorMessage = "Цена должна быть больше 1")]
        [Required(ErrorMessage = "Укажите цену")]
        public int Value { get; set; }

        [Required(ErrorMessage = "Укажите название покупки")]
        public string PurchaseName { get; set; }

        public PurchaseReason Reason { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Укажите категорию")]
        public PurchaseCategory Category { get; set; }

        public override string ToString()
        {
            return $"{PurchaseName}\t{Value}\t{Reason}\t{Date}\t{Category}";
        }

        public static Purchase Parse(string purchase)
        {
            var res = purchase.Split('\t');
            return new Purchase
            {
                Value = int.Parse(res[1]),
                PurchaseName = res[0],
                Reason = res[2].IsInt() ? (PurchaseReason)Enum.Parse(typeof(PurchaseReason), res[2]) : PurchaseReason.Need,
                Date = DateTime.Parse(res[3], DateTimeFormatInfo.InvariantInfo),
                Category = (PurchaseCategory)Enum.Parse(typeof(PurchaseCategory), res[4])
            };
        }

        public override int GetBalanceValue()
        {
            return -Value;
        }
    }
}
