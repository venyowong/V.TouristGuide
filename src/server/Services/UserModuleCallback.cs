using V.User.Models;
using V.User.Services;

namespace V.TouristGuide.Server.Services
{
    public class UserModuleCallback : IUserModuleCallback
    {
        private IConfiguration config;

        public UserModuleCallback(IConfiguration config)
        {
            this.config = config;
        }

        public Task OnOAuthLogin(HttpContext context, UserModel user)
        {
            context.Response.Redirect($"{this.config["OAuthCallbackUrl"]}?token={user.Token}");
            return Task.CompletedTask;
        }
    }
}
