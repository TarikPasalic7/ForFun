using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ForFun.API.Data;
using ForFun.API.Dtos;
using ForFun.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ForFun.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            _config = config;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userdto)
        {


            userdto.username = userdto.username.ToLower();
            if (await _repo.UserExusts(userdto.username))
                return BadRequest("This username already exist");

            var newuser = _mapper.Map<User>(userdto);
            var newcreateduser = await _repo.Register(newuser, userdto.password);
            var usertoreturn=_mapper.Map<UserDetailDto>(newcreateduser);

            return  CreatedAtRoute("GetUser",new{controller="Users",id=newcreateduser.Id},usertoreturn);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userdto)
        {

            var userlog = await _repo.Login(userdto.username.ToLower(), userdto.password);
            if (userlog == null)
                return Unauthorized();
            //Token method
            var claims = new[]
            {
                  new Claim(ClaimTypes.NameIdentifier,userlog.Id.ToString()),
                  new Claim(ClaimTypes.Name,userlog.Username)


            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokendescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokendescriptor);

            var user = _mapper.Map<UserforListDto>(userlog);
            return Ok(new
            {

                token = tokenhandler.WriteToken(token),
                user

            });
        }
    }
}