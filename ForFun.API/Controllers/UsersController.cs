using System.Threading.Tasks;
using ForFun.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ForFun.API.Dtos;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using ForFun.API.Helpers;
using ForFun.API.Models;

namespace ForFun.API.Controllers
{   [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IFunRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IFunRepository repo,IMapper mapper)
        {
            _repo=repo;
            _mapper=mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams){
             
             var currentUserID=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

             var userfromrepo=  await _repo.GetUser(currentUserID);

             userParams.UserId=userfromrepo.Id;

             if(string.IsNullOrEmpty(userParams.Gender)){
               
               userParams.Gender=userfromrepo.Gender == "male"? "female" :"male";

             }
          var users=await _repo.Getusers(userParams);
           var usertoreturn=_mapper.Map<IEnumerable<UserforListDto>>(users);
           Response.AddPagination(users.currentPage,userParams.PageSize,users.totalCount,users.totalPages);
          return Ok(usertoreturn);
            
        }


        [HttpGet("{id}",Name="GetUser")]
        public async Task<IActionResult> GetUser(int id){
           
           var user=await _repo.GetUser(id);
            var usertoreturn=_mapper.Map<UserDetailDto>(user);
           return Ok(usertoreturn);

        }
        [HttpPut("{id}")]

         public async Task<IActionResult> Updateuser(int id,UserForUpdateDto updateuserdto){
           
         if(id !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return  Unauthorized();

            var userFromRepo= await _repo.GetUser(id);
            _mapper.Map(updateuserdto,userFromRepo);
            if(await _repo.SaveAll())
            return NoContent();

            throw new Exception($"Updating user with id {id} faild");
        }

        [HttpPost("{id}/like/{recipientId}")]
 
        public async Task<IActionResult> LikeUser(int id,int recipientId) {

              if(id !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return  Unauthorized();

            var like= await _repo.GetLike(id,recipientId);
            if(like!= null)
            return BadRequest("You already liked this user");

            if(await _repo.GetUser(recipientId)==null)
            return NotFound();
        
        like= new Like{
        LikerId=id,
        LikeeId=recipientId

        };
         _repo.Add<Like>(like);

               if( await _repo.SaveAll())
               return Ok();

               return BadRequest("Cant like user");
        }
    }
}