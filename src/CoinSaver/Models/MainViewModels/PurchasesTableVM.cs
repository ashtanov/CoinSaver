using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models.MainViewModels
{
    public class HistoryTableVM
    {
        public IList<Record> Record { get; set; }
        public bool ShowCategoryColumn { get; set; }
    }
}
