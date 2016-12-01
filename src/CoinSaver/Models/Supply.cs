using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public class Supply : Record
    {
        [Range(1, int.MaxValue, ErrorMessage = "Величина поступления должна быть больше 1")]
        [Required(ErrorMessage = "Укажите величину поступления")]
        public int Value { get; set; }

        [Required(ErrorMessage = "Укажите название поступления")]
        public string SupplyName { get; set; }

        public override int GetBalanceValue()
        {
            return Value;
        }
    }
}
