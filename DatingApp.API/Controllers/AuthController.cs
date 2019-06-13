using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using DatingApp.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userRegisterDto){
            
            //validate request, create user
            userRegisterDto.Username = userRegisterDto.Username.ToLower();

            if (await _authRepo.UserExists(userRegisterDto.Username)) return BadRequest("Username already exists");

            var newUser = new User{
                UserName = userRegisterDto.Username
            };

            var createdUser = await _authRepo.Register(newUser, userRegisterDto.Password);

            return StatusCode(201); //createdAtRoute status code, will come back later

            
        }
    }
}