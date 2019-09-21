using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using DatingApp.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IConfiguration _config;
        private ILogger<AuthController> _logger;

        public AuthController(IAuthRepository authRepo, IConfiguration config, ILogger<AuthController> logger)
        {
            _authRepo = authRepo;
            _config = config;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userRegisterDto)
        {
            //validate request, create user
            userRegisterDto.Username = userRegisterDto.Username.ToLower();

            if (await _authRepo.UserExists(userRegisterDto.Username)) return BadRequest("Username already exists");

            var newUser = new User
            {
                UserName = userRegisterDto.Username
            };

            var createdUser = await _authRepo.Register(newUser, userRegisterDto.Password);

            return StatusCode(201); //createdAtRoute status code, will come back later

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userLoginDto)
        {

            try
            {

                var userFromRepo = await _authRepo.Login(userLoginDto.Username.ToLower(), userLoginDto.Password);

                if (userFromRepo == null)
                {

                    return Unauthorized();
                }

                //create claims
                var claims = new[] {
                new Claim (ClaimTypes.NameIdentifier, userFromRepo.Id.ToString ()),
                new Claim (ClaimTypes.Name, userFromRepo.UserName)
            };

                //hashed key to sign our token in order for the server to validate the token
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

                //generate signing credentials for encrypting token
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

                //create security token descriptor; contains claims, expir date, and signing credentials
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token)
                });
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error Logging in: {ex}");
                return StatusCode(500, "Problem Logging In");
            }

        }
    }
}