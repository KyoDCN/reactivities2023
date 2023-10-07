using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain;
using Reactivities.Server.DataTransferObjects;
using Reactivities.Server.Services;
using System.Security.Claims;

namespace Reactivities.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<ApplicationUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
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

            return CreateUserDTO(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if(await _userManager.Users.AnyAsync(x => x.UserName == registerDTO.Username))
            {
                ModelState.AddModelError("username", "Username is already taken.");
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == registerDTO.Email))
            {
                ModelState.AddModelError("email", "Email is already taken.");
            }

            if(ModelState.ErrorCount > 0)
            {
                return ValidationProblem();
            }

            var user = new ApplicationUser
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                UserName = registerDTO.Username
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                return CreateUserDTO(user);
            }

            return BadRequest(result.Errors);
        }

        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateUserDTO(user);
        }

        private UserDTO CreateUserDTO(ApplicationUser user) => new()
        {
            DisplayName = user.DisplayName,
            Image = null,
            Token = _tokenService.CreateToken(user),
            Username = user.UserName
        };
    }
}
