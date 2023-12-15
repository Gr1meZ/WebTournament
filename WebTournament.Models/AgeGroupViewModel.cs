using System.ComponentModel.DataAnnotations;
using FoolProof.Core;

namespace WebTournament.Models
{
    public class AgeGroupViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Поле возрастной группы не заполнено")]
        public string Name { get; set; }

        [Range(1, 100, ErrorMessage = "Поле минимального возраста не заполнено и должно быть от 1 до 100"), Required(ErrorMessage = "Минимальеный возраст должен быть заполнен")]

        public int? MinAge { get; set;}

        [Range(1, 100, ErrorMessage = "Поле максимального возраста не заполнено и должно быть от 1 до 100")]
        [GreaterThan("MinAge", ErrorMessage = "Максимальный возраст должен быть больше минимального"), Required(ErrorMessage = "Максимальный возраст должен быть заполнен")]
        public int? MaxAge { get; set; }
    }
}
