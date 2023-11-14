using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTournament.Models
{
    public class TournamentViewModel
    {
       
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Имя не заполнено")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]

        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Адрес не заполнен")]

        public string Address { get; set; }
    }
}
