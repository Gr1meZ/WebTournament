using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoolProof.Core;

namespace WebTournament.Models
{
    public class AgeGroupViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public int MinAge { get; set;}

        [Required(ErrorMessage = "Поле не заполнено")]
        public int MaxAge { get; set; }
    }
}
