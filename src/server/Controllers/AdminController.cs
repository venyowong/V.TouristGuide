using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using V.TouristGuide.Server.Attributes;
using V.TouristGuide.Server.Models;
using V.User.Attributes;
using V.User.Services;

namespace V.TouristGuide.Server.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : Controller
    {
        [HttpPost("login")]
        public object Login([FromBody] AdminLoginRequest request, [FromServices] IConfiguration config,
            [FromServices] JwtService jwt)
        {
            var pwdCfg = config["Admin:Password"];
            if (!string.IsNullOrEmpty(pwdCfg) && request.Pwd != pwdCfg)
            {
                return new { status = -1, msg = "密码错误" };
            }

            return new { status = 0, data = jwt.GenerateToken(new Dictionary<string, string> { { "role", "admin" } }) };
        }

        [HttpPost("file/upload")]
        [JwtValidation]
        [AdminRole]
        public async Task<object> UploadFile([FromServices] IWebHostEnvironment env, [FromServices] IConfiguration config)
        {
            var file = this.Request.Form.Files?.FirstOrDefault();
            if (file == null)
            {
                return new { status = -1, msg = "获取不到文件" };
            }

            var folder = Path.Combine(env.WebRootPath, "files");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            using (var fileStream = file.OpenReadStream())
            using (var stream = new FileStream(Path.Combine(folder, file.FileName),
                FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                await fileStream.CopyToAsync(stream);
            }
            return new { status = 0, data = new { value = $"{config["OAuth:BaseUrl"]}/files/{file.FileName}" } };
        }
    }
}
