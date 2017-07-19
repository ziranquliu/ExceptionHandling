using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ExceptionHandling.Models
{
    public class LoginInfo
    {
        [Display(Name ="User Name")]
        [Required(ErrorMessage = "User Name is manadatory!")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is manadatory!")]
        public string Password { get; set; }
    }
}