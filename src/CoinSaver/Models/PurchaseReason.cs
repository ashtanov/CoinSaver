using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public enum PurchaseReason
    {
        [Display(Name = "Не нужно")]
        Needless = 0,

        [Display(Name = "Не критично")]
        NotNeed = 1,

        [Display(Name = "Нужно")]
        Need = 2,

        [Display(Name = "Необходимо")]
        Must = 3
    }
}
