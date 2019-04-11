using System.ComponentModel.DataAnnotations;

namespace ForFun.API.Dtos
{
    public class UserForRegisterDto
    {

        [Required]
        public string username { get; set; }


        [Required]
        [StringLength(18,MinimumLength=6,ErrorMessage="You must type minimum 6 characters")]
        public string  password { get; set; }
    }
}