using CoinSaver.Models.MainViewModels;
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

        public HistoryTableVM PurchasesTable { get; set; }

        public int Balance { get; set; }
    }
}
