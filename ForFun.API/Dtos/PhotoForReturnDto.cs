using System;

namespace ForFun.API.Dtos
{
    public class PhotoForReturnDto
    {
          public int Id { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public  DateTime DateAdded { get; set; }
        public bool mainphoto { get; set; }
        public string PublicId { get; set; }
    }
}