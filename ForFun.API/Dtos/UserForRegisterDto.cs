using System.ComponentModel.DataAnnotations;
using System;
namespace ForFun.API.Dtos
{
    public class UserForRegisterDto
    {

        [Required]
        public string username { get; set; }


        [Required]
        [StringLength(18,MinimumLength=6,ErrorMessage="You must type minimum 6 characters")]
        public string  password { get; set; }

        [Required]        
        public string gender { get; set; }

        [Required]
        public DateTime birthDate { get; set; }

        [Required]
        public string city { get; set; }
        [Required]
        public string country { get; set; }

        public DateTime created { get; set; }
        public DateTime lastactive { get; set; }

        public UserForRegisterDto()
        {
            created=DateTime.Now;
            lastactive=DateTime.Now;
        }
        
    }
}