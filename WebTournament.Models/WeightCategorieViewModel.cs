using System.ComponentModel.DataAnnotations;

namespace WebTournament.Models
{
    public class WeightCategorieViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Возрастная категория не выбрана")]
        public Guid? AgeGroupId { get; set; }
        public string AgeGroupName { get; init; } = string.Empty;

        [Range(1, 100, ErrorMessage = "Предельный вес должен находиться в диапозоне от 1 до 100"), Required(ErrorMessage = "Предельный вес должнен быть заполнен")]
        public int? MaxWeight { get; init; } = 0; 
        [Required(ErrorMessage = "Название не заполнено")]
        public string WeightName { get; set; }
    }
}
