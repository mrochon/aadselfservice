using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AADSelfService.Models
{
    public class User
    {
        [Required]
        [RegularExpression(@"\w*", ErrorMessage ="Alpanumeric characters only")]
        public string Id { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        //[RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        [Display(Name = "Re-enter password")]
        public string RepeatPassword { get; set; }
    }
}
