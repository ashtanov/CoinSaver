using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public class IndexViewModel
    {
        public string Name { get; set; }

        public IEnumerable<Purchase> Purchases { get; set; }
    }
}
