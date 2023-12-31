﻿using System.ComponentModel.DataAnnotations;

namespace WebTournament.Presentation.MVC.ViewModels
{
    public class TrainerViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Клуб не выбран")]
        public Guid? ClubId { get; set; }

        public string ClubName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Имя не заполнено")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Фамилия не заполнена")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Отчество не заполнено")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Телефон не заполнен"), RegularExpression("[^_]*", ErrorMessage = "Заполните телефон полностью")]
        public string Phone { get; set; }
    }
}
