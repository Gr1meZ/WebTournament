using System.ComponentModel.DataAnnotations;

namespace WebTournament.Application.DTO
{
    public class TournamentDto
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
