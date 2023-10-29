using HealthERP.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HealthERP.Infrasctructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
    }
}
