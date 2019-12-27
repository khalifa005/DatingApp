using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repo;
        private readonly IConfiguration config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this.config = config;
            this.repo = repo;

        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if (await repo.UserExist(userForRegisterDto.Username))
                return BadRequest("userExist");

            var userTocreateUser = new User
            {
                Username = userForRegisterDto.Username
            };
            var createdUser = await repo.Register(userTocreateUser, userForRegisterDto.Password);
            return StatusCode(201);

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var logedinUser = await repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (logedinUser == null)
                return Unauthorized();

            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,logedinUser.Id.ToString()),
                new Claim(ClaimTypes.Name,logedinUser.Username)
            };

            var key =new SymmetricSecurityKey(Encoding.UTF8
             .GetBytes(config.GetSection("AppSettings:Token").Value));

             var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

             var tokenDescriber=new SecurityTokenDescriptor{
                 Subject=new ClaimsIdentity(claims),
                 Expires=DateTime.Now.AddDays(1),
                 SigningCredentials=creds
             };
             var tokenHandller=new JwtSecurityTokenHandler();
             var token=tokenHandller.CreateToken(tokenDescriber);
             return Ok(new{
                 token=tokenHandller.WriteToken(token)
             });


        }



    }
}