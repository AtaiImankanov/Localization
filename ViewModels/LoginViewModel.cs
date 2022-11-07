using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace lavAspMvclast.ViewModels
{
    public class LoginViewModel
    {
        [Required]

        [Display(Name = "Email")]
        [Remote(action: "EmailValid", controller:"ToDoTasks")]
        public string Email { get; set; }



        [Required]

        [DataType(DataType.Password)]

        [Display(Name = "Пароль")]

        public string Password { get; set; }



        [Display(Name = "Запомнить?")]

        public bool RememberMe { get; set; }



        public string ReturnUrl { get; set; }
    }
}
