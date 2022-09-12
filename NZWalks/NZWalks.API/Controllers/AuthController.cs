using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenHandler _tokenHandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            _userRepository = userRepository;
            _tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
        {
            // Check if user is authentificated

            // Check username and password
            var user = await _userRepository.AuthentificateAsync(loginRequest.Username, loginRequest.Password);

            if (user!=null)
            {
                var token = await _tokenHandler.CreateTokenAsyn(user);
                return Ok(token);
            }

            return BadRequest("Username or Password is incorrect");
        }
    }
}
