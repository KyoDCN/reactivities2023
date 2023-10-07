using Microsoft.AspNetCore.Http;
using Reactivities.Application.Interfaces;
using System.Security.Claims;

namespace Reactivities.Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserName()
        {
            string? username = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

            if(username == null) return string.Empty;

            return username;
        }
    }
}
