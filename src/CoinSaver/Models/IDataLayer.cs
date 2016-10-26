using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public interface IDataLayer
    {
        Task SaveSpendingAsync(string name, Purchase spanding);
        IEnumerable<Purchase> GetSpendings(string name);
    }
}
