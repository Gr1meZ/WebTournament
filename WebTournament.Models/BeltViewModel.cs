using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTournament.Models
{
    public class BeltViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public int BeltNumber { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string ShortName { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string FullName { get; set; }
    }
}
