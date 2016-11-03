using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models.AccountViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Имя")]
        public string RealName { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Wrong")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}
