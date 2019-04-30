using System;
namespace ForFun.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public  DateTime DateAdded { get; set; }
        public bool mainphoto { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}