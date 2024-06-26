﻿using project_get_discount_back._1_Domain.Interfaces;
using System.Security.Claims;

namespace project_get_discount_back._1_Domain.Service.Usuario
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string ObterUsername()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var nomeClaim = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var sobrenomeClaim = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
            return $"{nomeClaim?.Value} {sobrenomeClaim?.Value}";
        }
    }
}
