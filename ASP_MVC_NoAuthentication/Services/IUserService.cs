﻿using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface IUserService
    {
        public Task<User> GetUserByName(string name);

        public Task SaveSettings(User user);
    }
}
