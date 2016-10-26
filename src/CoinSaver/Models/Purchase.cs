using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public class Purchase
    {
        public int price { get; set; }
        public string purshaseName { get; set; }
        private string _reason;
        public string reason {
            get
            {
                return _reason;
            }
            set
            {
                _reason = value?.Replace("\n", " ")?.Replace("\t", " ")?.Replace("@@@", " ");
            }
        }
        public DateTime date { get; set; }

        public override string ToString()
        {
            return $"{purshaseName}\t{price}\t{reason}\t{date}";
        }

        public static Purchase Parse(string purchase)
        {
            var res = purchase.Split('\t');
            return new Purchase { price = int.Parse(res[1]), purshaseName = res[0], reason = res[2], date = DateTime.Parse(res[3]) };
        }
    }
}
