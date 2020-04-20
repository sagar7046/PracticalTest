using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Test.Models
{
    public class UserViewModel
    {
        [Display(Name="Username")]
        [Required(ErrorMessage="This field is required")]
        [RegularExpression("^[A-Za-z 0-9]*$",ErrorMessage="Only letters are allowed")]
        public string Username { get; set; }

        [Display(Name="Password")]
        [Required(ErrorMessage="This field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name="Photo")]        
        [Required(ErrorMessage="This field is required")]
        [DataType(DataType.Upload)]
        public IFormFile ProfileImage { get; set; }

        [Required(ErrorMessage="This field is required")]
        [Display(Name="Curriculum Vitae (CV)")]
        [DataType(DataType.Upload)]
        public IFormFile Resume { get; set; }

        [Display(Name="Mobile")]
        [Required(ErrorMessage="This field is required")]
        [DataType(DataType.PhoneNumber,ErrorMessage="Please enter valid phone")]  
        [RegularExpression("^[+0-9]{10,}$",ErrorMessage="Only digits are allowed")]      
        public string MobileNumber { get; set; }
    }
}