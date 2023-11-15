using System.ComponentModel.DataAnnotations;

namespace WebTournament.Models
{
    public class ClubViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        public string Name { get; set; }

    }
}
