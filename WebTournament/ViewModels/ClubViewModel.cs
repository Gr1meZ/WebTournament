using System.ComponentModel.DataAnnotations;

namespace WebTournament.Presentation.MVC.ViewModels
{
    public class ClubViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        public string Name { get; set; }

    }
}
