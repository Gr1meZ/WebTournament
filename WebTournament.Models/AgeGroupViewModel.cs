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
        [Required(ErrorMessage = "Поле возрастной категории не заполнено")]
        public string Name { get; set; }

        [Range(1, 100, ErrorMessage = "Поле минимального возраста не заполнено и должно быть от 1 до 100")]

        public int MinAge { get; set;}

        [Range(1, 100, ErrorMessage = "Поле максимального возраста не заполнено и должно быть от 1 до 100")]
        [GreaterThan("MinAge", ErrorMessage = "Максимальный возраст должен быть больше минимального")]
        public int MaxAge { get; set; }
    }
}
