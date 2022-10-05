using Microsoft.AspNetCore.Mvc;
using V.TouristGuide.Server.Attributes;
using V.TouristGuide.Server.Models;
using V.TouristGuide.Server.Services;
using V.User.Attributes;

namespace V.TouristGuide.Server.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        [JwtValidation]
        [UserId]
        [HttpGet("operations")]
        public async Task<UserOperationsResponse> GetOperations([FromServices] OperationService operationService,
            [FromServices] PlaceService placeService)
        {
            var userId = long.Parse(this.HttpContext.Items["userId"].ToString());
            var places = await placeService.GetAllPlaces();
            var operations = await operationService.GetUserOperations(userId);
            return new UserOperationsResponse
            {
                Collections = operations.Where(o => o.Type == 0)
                    .Where(o => places.Any(p => p.Id == o.PlaceId))
                    .OrderByDescending(o => o.UpdateTime)
                    .Select(o =>
                    {
                        var place = places.First(p => p.Id == o.PlaceId);
                        return new UserOperationsResponse.Operation
                        {
                            Id = o.Id,
                            PlaceId = o.PlaceId,
                            UpdateTime = o.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            Name = place.Name,
                            Cover = place.Cover
                        };
                    }).ToList(),
                Comments = operations.Where(o => o.Type == 1)
                    .Where(o => places.Any(p => p.Id == o.PlaceId))
                    .OrderByDescending(o => o.UpdateTime)
                    .Select(o =>
                    {
                        var place = places.First(p => p.Id == o.PlaceId);
                        return new UserOperationsResponse.Operation
                        {
                            Id = o.Id,
                            PlaceId = o.PlaceId,
                            Remark = o.Remark,
                            UpdateTime = o.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            Name = place.Name,
                            Cover = place.Cover
                        };
                    }).ToList()
            };
        }

        [HttpPost("file/upload")]
        [JwtValidation]
        [UserId]
        public async Task<object> UploadFile([FromServices] IWebHostEnvironment env)
        {
            var file = this.Request.Form.Files?.FirstOrDefault();
            if (file == null)
            {
                return new { status = -1, msg = "获取不到文件" };
            }

            var folder = Path.Combine(env.WebRootPath, "files/user");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var fileName = Guid.NewGuid().ToString("N") + file.FileName.Substring(file.FileName.LastIndexOf('.'));
            using (var fileStream = file.OpenReadStream())
            using (var stream = new FileStream(Path.Combine(folder, fileName),
                FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                await fileStream.CopyToAsync(stream);
            }
            return new { status = 0, data = new { value = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/files/user/{fileName}" } };
        }
    }
}
