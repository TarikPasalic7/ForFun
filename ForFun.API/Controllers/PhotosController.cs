using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ForFun.API.Data;
using ForFun.API.Dtos;
using ForFun.API.Helpers;
using ForFun.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ForFun.API.Controllers
{

    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
       private readonly IFunRepository _repo;
       private readonly IMapper _maper;
       private readonly IOptions<CloudinarySettings> _CloudinaryOptions;
       private Cloudinary _cloudinary;

        public PhotosController(IFunRepository repo,IMapper maper,IOptions<CloudinarySettings> CloudinaryOptions )
        {
            _repo=repo;
            _maper=maper;
            _CloudinaryOptions=CloudinaryOptions;
            Account acc=new Account(
                _CloudinaryOptions.Value.CloudName,
                _CloudinaryOptions.Value.ApiKey,
                _CloudinaryOptions.Value.ApiSecret

            );
            _cloudinary=new Cloudinary(acc);
        }
       [HttpGet("{id}",Name="GetPhoto")]
          
          public async Task<IActionResult> GetPhoto(int id){

            var photorepo=await _repo.GetPhoto(id);

            var photo=_maper.Map<PhotoForReturnDto>(photorepo);
            return Ok(photo);

          }

       [HttpPost]

       public async Task<IActionResult> AddPhotoUser(int userId,[FromForm]PhotoCreatingDto photodto )
       {
          
               if(userId !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return  Unauthorized();
   
        
            var userFromRepo= await _repo.GetUser(userId);

            var file=photodto.File;

            var Uploadresults= new ImageUploadResult();

            if(file.Length>0)
            {
              using(var stream=file.OpenReadStream())
              {

                  var uploadparams=new ImageUploadParams(){

                      File=new FileDescription(file.Name,stream),
                      Transformation= new Transformation().Width("500").Height("500").Crop("fill").Gravity("face")
                  };
                    Uploadresults=_cloudinary.Upload(uploadparams);
              }

            }
           photodto.URL=Uploadresults.Uri.ToString();
           photodto.PublicId=Uploadresults.PublicId;

           var photo=_maper.Map<Photo>(photodto);
            if(!userFromRepo.Photos.Any(p=>p.mainphoto))
            photo.mainphoto=true;

            userFromRepo.Photos.Add(photo);

            
            if(await _repo.SaveAll())
            {
                var returnphoto=_maper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto",new {id=photo.Id},returnphoto);
            }
             return BadRequest("Could not add the photo");
       }
      
      [HttpPost("{id}/setMain")]
       
       public async Task<IActionResult> setMainPhoto(int userId, int id) {

     if(userId !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return  Unauthorized();
            
            var user= await _repo.GetUser(userId);
            if(!user.Photos.Any(p=>p.Id==id))
            return Unauthorized();

            var photfromrepo=await _repo.GetPhoto(id);

            if(photfromrepo.mainphoto)
            return BadRequest("This is already the main photo");

            var currentmainphoto= await _repo.GetMainPhotoUser(userId);
            currentmainphoto.mainphoto=false;
            photfromrepo.mainphoto=true;
            if(await _repo.SaveAll())
            return NoContent();

            return BadRequest("Could not set as main");
       }
         

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeletePhoto(int userId, int id) {

          if(userId !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return  Unauthorized();

            var user= await _repo.GetUser(userId);
            if(!user.Photos.Any(p=>p.Id==id))
            return Unauthorized();

            var photfromrepo=await _repo.GetPhoto(id);

            if(photfromrepo.mainphoto)
            return BadRequest("You can not delete your main photo");
             
             if(photfromrepo.PublicId!=null){
                 var deleteparamas= new DeletionParams(photfromrepo.PublicId);
            var result=_cloudinary.Destroy(deleteparamas);
            if(result.Result=="ok") {
              _repo.Delete(photfromrepo);
            }

             }
             if(photfromrepo.PublicId==null)
             {
               _repo.Delete(photfromrepo);
             }

            
            if(await _repo.SaveAll()) {
              return Ok();
            }
             return BadRequest("Failed to delete the photo");
        }

    }
}