using System.ComponentModel.DataAnnotations;

namespace WebTournament.Presentation.MVC.ViewModels
{
    public class FighterViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Не выбран турнир")]
        public Guid? TournamentId { get; set; }
        [Required(ErrorMessage = "Не выбрана весовая категория")]
        public Guid? WeightCategorieId { get; set; }
        [Required(ErrorMessage = "Не выбран пояс")]
        public Guid? BeltId { get; set; }
        [Required(ErrorMessage = "Не выбран тренер")]
        public Guid? TrainerId { get; set; }
     
        [Required(ErrorMessage = "Имя не заполнено")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Фамилия не заполнена")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Дата рождения не заполнена")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Возраст не заполнен")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Страна не заполнена")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Город не заполнен")]
        public string City { get; set; }

        [Required(ErrorMessage = "Пол не заполнен")]
        public string Gender { get; set; }

        //for reading
        public string TournamentName { get; init; } = string.Empty;
        public string WeightCategorieName { get; init; } = string.Empty;
        public int WeightNumber { get; set; } 
        public string? BeltShortName { get; set; } 
        public int BeltNumber { get; set; }
        public string? TrainerName { get; set; } 
        public string? ClubName { get; set; } 
        
    }
}
