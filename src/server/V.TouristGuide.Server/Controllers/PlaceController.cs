using Microsoft.AspNetCore.Mvc;
using V.TouristGuide.Server.Attributes;
using V.TouristGuide.Server.Entities;
using V.TouristGuide.Server.Helpers;
using V.TouristGuide.Server.Models;
using V.TouristGuide.Server.Services;
using V.User.Attributes;
using V.Common.Extensions;
using V.User.Services;

namespace V.TouristGuide.Server.Controllers
{
    [ApiController]
    [Route("place")]
    public class PlaceController : Controller
    {
        private PlaceService service;
        private OperationService operationService;

        public PlaceController(PlaceService placeService, OperationService operationService)
        {
            this.service = placeService;
            this.operationService = operationService;
        }

        [HttpGet("list")]
        public async Task<object> GetPagedList([FromQuery] AdminCrudRequest request)
        {
            var places = await this.service.GetAllPlaces();
            if (places.IsNullOrEmpty())
            {
                return new
                {
                    status = 0,
                    data = new
                    {
                        total = 0,
                        items = new List<Place>()
                    }
                };
            }

            IEnumerable<Place> result = null;
            if (string.IsNullOrWhiteSpace(request.OrderDir))
            {
                result = places;
            }
            else
            {
                if (request.OrderDir == "asc")
                {
                    result = places.OrderBy(p => ReflectionHelper.GetValue(p, request.OrderBy));
                }
                else
                {
                    result = places.OrderByDescending(p => ReflectionHelper.GetValue(p, request.OrderBy));
                }
            }
            return new
            {
                status = 0,
                data = new
                {
                    total = places.Count,
                    items = result.Skip((request.Page - 1) * request.PerPage).Take(request.PerPage).ToList()
                }
            };
        }

        [HttpGet("nearby")]
        public async Task<object> GetNearbyList(decimal lng, decimal lat, int pageIndex, int pageCap, double range = 0)
        {
            var places = await this.service.GetAllPlaces();
            if (places.IsNullOrEmpty())
            {
                return Result.Success(new List<PlaceModel>());
            }

            return this.GetPlacesResult(places, lng, lat, pageIndex, pageCap, range);
        }

        [HttpGet("search")]
        public async Task<object> Search(string keyword, decimal lng, decimal lat, int pageIndex, int pageCap, double range = 0)
        {
            var places = await this.service.GetAllPlaces();
            if (places.IsNullOrEmpty())
            {
                return Result.Success(new List<PlaceModel>());
            }

            places = places.FindAll(p => p.Name.Contains(keyword));
            return this.GetPlacesResult(places, lng, lat, pageIndex, pageCap, range);
        }

        [HttpGet("addition/list")]
        public async Task<object> GetPagedAdditions([FromQuery] AdminCrudRequest request)
        {
            var additions = await this.service.GetAllPlaceAdditions();
            if (additions.IsNullOrEmpty())
            {
                return new
                {
                    status = 0,
                    data = new
                    {
                        total = 0,
                        items = new List<PlaceAddition>()
                    }
                };
            }

            IEnumerable<PlaceAddition> result = null;
            if (string.IsNullOrWhiteSpace(request.OrderDir))
            {
                result = additions;
            }
            else
            {
                if (request.OrderDir == "asc")
                {
                    result = additions.OrderBy(p => ReflectionHelper.GetValue(p, request.OrderBy));
                }
                else
                {
                    result = additions.OrderByDescending(p => ReflectionHelper.GetValue(p, request.OrderBy));
                }
            }
            return new
            {
                status = 0,
                data = new
                {
                    total = additions.Count,
                    items = result.Skip((request.Page - 1) * request.PerPage).Take(request.PerPage).ToList()
                }
            };
        }

        [HttpGet("detail")]
        public async Task<PlaceDetail> GetDetail(long id, [FromServices] UserService userService, long userId = 0)
        {
            var detail = await this.service.GetPlaceDetail(id);
            if (detail == null)
            {
                return null;
            }

            var operations = await this.operationService.GetPlaceOperations(id);
            detail.HasStar = operations.Any(o => o.Type == 0 && o.UserId == userId);
            detail.Stars = operations.Count(o => o.Type == 0);
            detail.CommentOperations = new List<Comment>();
            foreach (var operation in operations.Where(o => o.Type == 1).OrderByDescending(o => o.UpdateTime))
            {
                var user = await userService.GetUser(operation.UserId);
                detail.CommentOperations.Add(new Comment
                {
                    Id = operation.Id,
                    UserId = operation.UserId,
                    Content = operation.Remark,
                    Name = user.Name,
                    Avatar = user.Avatar
                });
            }
            return detail;
        }

        [JwtValidation]
        [UserId]
        [HttpPost("comment")]
        public async Task<object> Comment([FromBody] PlaceCommentRequest request)
        {
            if (request.PlaceId < 0)
            {
                return new Result { Code = -1, Msg = "评论对象不存在" };
            }

            var userId = long.Parse(this.HttpContext.Items["userId"].ToString());
            var result = await this.operationService.AddOperation(new Operation
            {
                UserId = userId,
                PlaceId = request.PlaceId,
                Type = 1,
                Remark = request.Comment
            });
            return new Result { Code = result ? 0 : -1 };
        }

        [JwtValidation]
        [HttpPost("comment/delete")]
        public async Task<object> DeleteComment([FromQuery] long id)
        {
            var userId = long.Parse(this.HttpContext.Items["userId"].ToString());
            var operations = await this.operationService.GetUserOperations(userId);
            var operation = operations.FirstOrDefault(o => o.Id == id && o.Type == 1);
            if (operation == null)
            {
                return new Result { Code = -1, Msg = "评论不存在" };
            }

            var result = await this.operationService.DeleteOperation(id);
            return new Result { Code = result ? 0 : -1 };
        }

        [JwtValidation]
        [UserId]
        [HttpPost("collect")]
        public async Task<object> Collect([FromQuery] long placeId)
        {
            if (placeId < 0)
            {
                return new Result { Code = -1, Msg = "收藏对象不存在" };
            }

            var userId = long.Parse(this.HttpContext.Items["userId"].ToString());
            var operations = await this.operationService.GetUserOperations(userId);
            var operation = operations.FirstOrDefault(o => o.PlaceId == placeId && o.Type == 0);
            bool result;
            if (operation != null)
            {
                result = await this.operationService.DeleteOperation(operation.Id);
            }
            else
            {
                result = await this.operationService.AddOperation(new Operation
                {
                    UserId = userId,
                    PlaceId = placeId,
                    Type = 0
                });
            }
            return new Result { Code = result ? 0 : -1 };
        }

        #region admin
        [JwtValidation]
        [AdminRole]
        [HttpPost("admin/add")]
        public async Task<object> AddPlace(AdminPlaceModel p)
        {
            var result = await this.service.AddPlace(new Place
            {
                Name = p.Name,
                Cover = p.Cover,
                OpenTime = p.OpenTime,
                Address = p.Address.Address,
                Lng = p.Address.Lng,
                Lat = p.Address.Lat,
                Tel = p.Tel,
                Type = p.Type,
                Region = p.Address.City
            });
            return new { status = result ? 0 : -1 };
        }

        [JwtValidation]
        [AdminRole]
        [HttpPost("admin/update")]
        public async Task<object> UpdatePlace([FromBody] AdminPlaceModel p, [FromQuery] long id)
        {
            var place = await this.service.GetPlaceById(id);
            if (place == null)
            {
                return new { status = -1 };
            }

            place.Name = p.Name;
            place.Cover = p.Cover;
            place.OpenTime = p.OpenTime;
            place.Tel = p.Tel;
            place.Type = p.Type;
            place.Visibility = p.Visibility;
            var result = await this.service.UpdatePlace(place);
            return new { status = result ? 0 : -1 };
        }

        [JwtValidation]
        [AdminRole]
        [HttpPost("admin/delete")]
        public async Task<object> DeletePlace([FromQuery] long id)
        {
            var result = await this.service.DeletePlace(id);
            return new { status = result ? 0 : -1 };
        }

        [JwtValidation]
        [AdminRole]
        [HttpPost("addition/admin/add")]
        public async Task<object> AddPlaceAddition(PlaceAddition p)
        {
            var result = await this.service.AddPlaceAddition(p);
            return new { status = result ? 0 : -1 };
        }

        [JwtValidation]
        [AdminRole]
        [HttpPost("addition/admin/update")]
        public async Task<object> UpdatePlaceAddition(PlaceAddition p)
        {
            var result = await this.service.UpdatePlaceAddition(p);
            return new { status = result ? 0 : -1 };
        }

        [JwtValidation]
        [AdminRole]
        [HttpPost("addition/admin/delete")]
        public async Task<object> DeletePlaceAddition([FromQuery] long id)
        {
            var result = await this.service.DeletePlaceAddition(id);
            return new { status = result ? 0 : -1 };
        }
        #endregion

        private PagedResult<PlaceModel> GetPlacesResult(List<Place> places, decimal lng, decimal lat, int pageIndex, int pageCap, double range)
        {
            var result = places.Select(p =>
            {
                var model = new PlaceModel(p);
                model.Distance = CaculationHelper.GetDistance((double)lng, (double)lat, (double)p.Lng, (double)p.Lat);
                return model;
            }).ToList();
            if (range > 0)
            {
                result = result.FindAll(x => x.Distance <= range && x.Distance <= x.Visibility);
            }
            return new PagedResult<PlaceModel>
            {
                Data = result.OrderBy(x => x.Distance)
                    .Skip(pageIndex * pageCap)
                    .Take(pageCap)
                    .ToList(),
                TotalCount = result.Count,
                PageCount = Math.Ceiling((double)result.Count / pageCap)
            };
        }
    }
}
