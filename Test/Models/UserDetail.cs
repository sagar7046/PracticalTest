using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Test.Models
{
    public class UserDetail
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="Username")]
        public string Username { get; set; }

        [Display(Name="Password")]
        public string Password { get; set; }

        [Display(Name="Photo")]
        public string ProfileImage { get; set; }

        [Display(Name="Curriculum Vitae (CV)")]
        public string Resume { get; set; }

        [Display(Name="Mobile")]
        public string MobileNumber { get; set; }
    }
}