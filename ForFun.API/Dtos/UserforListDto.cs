
using System;
using System.Collections.Generic;
namespace ForFun.API.Dtos
{
    public class UserforListDto
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
    }
}