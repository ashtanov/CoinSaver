using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public enum PurchaseCategory
    {
        [Display(Name = "Другое")]
        Other = 0,

        [Display(Name = "Кафе и рестораны")]
        Cafe = 1,

        [Display(Name = "Гипермаркет")]
        Shop = 2,

        [Display(Name = "Аптека")]
        Medicine = 3,

        [Display(Name = "Городской транспорт")]
        Transport = 4,

        [Display(Name = "Одежда и обувь")]
        Clothes = 5,

        [Display(Name = "Развлечения")]
        Entertainments = 6,

        [Display(Name = "Путешествия")]
        Travels = 7,

        [Display(Name = "Красота и здоровье")]
        Beauty = 8,

        [Display(Name ="Учеба и хобби")]
        SelfDevelopment = 9
    }
}
