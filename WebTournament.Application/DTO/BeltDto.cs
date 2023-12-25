using System.ComponentModel.DataAnnotations;

namespace WebTournament.Application.DTO
{
    public class BeltDto
    {
        public Guid Id { get; set; }
        [Range(1, 10, ErrorMessage = "Номер пояса должен быть от {1} до {2}"), Required(ErrorMessage = "Номер пояса должен быть заполнен")]
        public int? BeltNumber { get; set; }
        [Required(ErrorMessage = "Сокращенное название не заполнено")]
        public string ShortName { get; set; }
        [Required(ErrorMessage = "Полное название не заполнено")]
        public string FullName { get; set; }
    }
}
