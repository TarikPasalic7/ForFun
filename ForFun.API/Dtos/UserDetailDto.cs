        using System;
using System.Collections.Generic;
using ForFun.API.Models;

namespace ForFun.API.Dtos
{
    public class UserDetailDto
    {
    public int  Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }

        public int age {get;set;}
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        
        public string City { get; set; }
        public string Country { get; set; }

       public string photoURL{ get; set; } 
       public ICollection<PhotoDetailsDto> Photos { get; set; }
    }
}