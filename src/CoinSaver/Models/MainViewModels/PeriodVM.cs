using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public class PeriodVM : IValidatableObject
    {
        public PeriodVM()
        {
            Start = DateTime.Now.AddDays(-1);
            End = DateTime.Now;
            IsActive = false;
        }
        public bool IsActive { get; set; }

        [Display(Name = "Начало")]
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }

        [Display(Name = "Конец")]
        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (End < Start)
                yield return new ValidationResult("Дата начала должна быть меньше даты конца");
        }
    }
}
