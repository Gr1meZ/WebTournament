using System.ComponentModel.DataAnnotations;

namespace WebTournament.Application.DTO
{
    public class ClubDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        public string Name { get; set; }

    }
}
