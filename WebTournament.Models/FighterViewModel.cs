using DataAccess.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


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
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public int Age { get; init; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string City { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        public string Gender { get; set; }

        //for reading
        public string TournamentName { get; init; } = string.Empty;
        public string WeightCategorieName { get; init; } = string.Empty;
        public string BeltName { get; init; } = string.Empty;
        public string TrainerName { get; init; } = string.Empty;
        public string ClubName { get; init; } = string.Empty;
    }
}
