using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Domain;
using Reactivities.Server.DataTransferObjects;

namespace Reactivities.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                return Unauthorized();
            }

            var isAuthenticated = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!isAuthenticated)
            {
                return Unauthorized();
            }

            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Image = null,
                Token = "this will be a token",
                Username = user.UserName
            };
        }
    }
}
