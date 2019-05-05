using System.Threading.Tasks;
using ForFun.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ForFun.API.Dtos;
using System.Collections.Generic;

namespace ForFun.API.Controllers
{
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
        public async Task<IActionResult> GetUsers(){

          var users=await _repo.Getusers();
           var usertoreturn=_mapper.Map<IEnumerable<UserforListDto>>(users);
          return Ok(usertoreturn);
            
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id){
           
           var user=await _repo.GetUser(id);
            var usertoreturn=_mapper.Map<UserDetailDto>(user);
           return Ok(usertoreturn);

        }
    }
}