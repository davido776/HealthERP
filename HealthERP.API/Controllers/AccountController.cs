using HealthERP.API.Dtos;
using HealthERP.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManger,
                                 IConfiguration Configuration)
        {
            _userManager = userManager;
            _signInManager = signInManger;
            configuration = Configuration;
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

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superdupersecretsuperdupersecretjjgfjdfgudfjgdgfudfudfgudftudgfudftudfgduftudfgduftdufgdufd"));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.UtcNow.AddDays(7),
            //    SigningCredentials = creds
            //};



            //var tokenHandler = new JwtSecurityTokenHandler();

            //var token = tokenHandler.CreateToken(tokenDescriptor);

            //return tokenHandler.WriteToken(token);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superdupersecretsuperdupersecretjjgfjdfgudfjgdgfudfudfgudftudgfudftudfgduftudfgduftdufgdufd"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
