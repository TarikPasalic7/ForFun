using System;
using Microsoft.AspNetCore.Http;

namespace ForFun.API.Dtos
{
    public class PhotoCreatingDto
    {
         public string URL { get; set; }
         public IFormFile File { get; set; }
          public string Description { get; set; }
         
         public  DateTime DateAdded { get; set; }

         public string PublicId { get; set; }


         public PhotoCreatingDto()
         {
             DateAdded=DateTime.Now;
         }
    }
}