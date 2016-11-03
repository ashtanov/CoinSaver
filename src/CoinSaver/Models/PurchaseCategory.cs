using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public enum PurchaseCategory
    {
        [Color("#cfcfd1")]
        [Display(Name = "Другое")]
        Other = 0,

        [Color("#d8a245")]
        [Display(Name = "Кафе и рестораны")]
        Cafe = 1,

        [Color("#3bafe5")]
        [Display(Name = "Гипермаркет")]
        Shop = 2,

        [Color("#62d68b")]
        [Display(Name = "Аптека")]
        Medicine = 3,

        [Color("#d1d11f")]
        [Display(Name = "Городской транспорт")]
        Transport = 4,

        [Color("#bc79e5")]
        [Display(Name = "Одежда и обувь")]
        Clothes = 5,

        [Color("#f92525")]
        [Display(Name = "Развлечения")]
        Entertainments = 6,

        [Color("#97ea07")]
        [Display(Name = "Путешествия")]
        Travels = 7,

        [Color("#f271a9")]
        [Display(Name = "Красота и здоровье")]
        Beauty = 8
    }
}
