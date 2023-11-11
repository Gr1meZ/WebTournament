using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTournament.Models
{
    public class ClubViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        public string Name { get; set; }

    }
}
