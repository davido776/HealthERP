using HealthERP.Application.Command.Administrators;
using HealthERP.Application.Command.PolicyHolders;
using HealthERP.Domain.Identity;
using HealthERP.Presentation.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthERP.Presentation.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManger)
        {
            _userManager = userManager;
            _signInManager = signInManger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();
        }

        private UserDto CreateUserObject(ApplicationUser user)
        {
            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = CreateToken(user),
            };
        }

        private string CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var roles = _userManager.GetRolesAsync(user).Result;


            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superdupersecretsuperdupersecretjjgfjdfgudfjgdgfudfudfgudftudgfudftudfgduftudfgduftdufgdufd"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        [HttpPost("admin")]
        public async Task<IActionResult> CreateAdministrator([FromBody] CreateAdministratorCommand request)
        {
            //return Ok();
            return HandleResult(await Mediator.Send(request));
        }

        [HttpPost("policy-holder")]
        public async Task<IActionResult> CreatePolicyHolder([FromBody] CreatePoliyholderCommand request)
        {
            return HandleResult(await Mediator.Send(request));
        }
    }
}
