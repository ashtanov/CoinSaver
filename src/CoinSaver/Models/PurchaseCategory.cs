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
        Other,
        [Display(Name = "Кафе и рестораны")]
        Cafe,
        [Display(Name = "Магазины")]
        Shop,
        [Display(Name = "Аптека")]
        Medicine,
        [Display(Name = "Городской транспорт")]
        Transport,
        [Display(Name = "Одежда и обувь")]
        Clothes,
        [Display(Name = "Развлечения")]
        Entertainments,
        [Display(Name = "Путешествия")]
        Travels
    }
}
