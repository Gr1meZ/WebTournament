using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTournament.Models
{
    public class WeightCategorieViewModel
    {
        public Guid Id { get; set; }
        public Guid AgeGroupId { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string AgeGroupName { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public int MaxWeight { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string WeightName { get; set; }
    }
}
