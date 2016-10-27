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
        Other,

        [Color("#d8a245")]
        [Display(Name = "Кафе и рестораны")]
        Cafe,

        [Color("#3bafe5")]
        [Display(Name = "Гипермаркет")]
        Shop,

        [Color("#62d68b")]
        [Display(Name = "Аптека")]
        Medicine,

        [Color("#d1d11f")]
        [Display(Name = "Городской транспорт")]
        Transport,

        [Color("#bc79e5")]
        [Display(Name = "Одежда и обувь")]
        Clothes,

        [Color("#f92525")]
        [Display(Name = "Развлечения")]
        Entertainments,

        [Color("#97ea07")]
        [Display(Name = "Путешествия")]
        Travels,

        [Color("#f271a9")]
        [Display(Name = "Красота и здоровье")]
        Beauty
    }
}
