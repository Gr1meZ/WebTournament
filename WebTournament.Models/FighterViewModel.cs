using DataAccess.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebTournament.Models
{
    public class FighterViewModel
    {
  
        public Guid Id { get; set; }
        [Required]
        public Guid TournamentId { get; set; }
        [Required]
        public Guid WeightCategorieId { get; set; }
        [Required]
        public Guid BeltId { get; set; }
        [Required]
        public Guid TrainerId { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string TournamentName { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string WeightCategorieName { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string BeltName { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string TrainerName { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string City { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Gender { get; set; }
    }
}
