using MH.Services.Interfaces;
using MH.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Services
{
    public static class ServiceDependencyRegister
    {
        public static void ServiceDependencyResolver(this IServiceCollection services)
        {
            services.AddScoped<IAccessTokenService, AccessTokenService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IUserSessionService, UserSessionService>();
            services.AddScoped<ISystemUserService, SystemUserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IActionsService, ActionsService>();
        }
    }
}
