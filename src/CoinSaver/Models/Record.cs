using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public abstract class Record
    {
        public DateTime Date { get; set; }

        public abstract int GetBalanceValue();
    }
}
