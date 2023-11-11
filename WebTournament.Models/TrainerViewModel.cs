using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTournament.Models
{
    public class TrainerViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]

        public Guid ClubId { get; set; }

        public string ClubName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Поле не заполнено")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        public string Phone { get; set; }
    }
}
