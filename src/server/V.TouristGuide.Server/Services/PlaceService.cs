using V.TouristGuide.Server.Daos;
using V.TouristGuide.Server.Entities;
using V.TouristGuide.Server.Helpers;
using V.TouristGuide.Server.Models;

namespace V.TouristGuide.Server.Services
{
    public class PlaceService
    {
        private const string _allPlacesCache = "V.TouristGuide.Server.Services.PlaceService.AllPlaces";
        private const string _allPlaceAdditionsCache = "V.TouristGuide.Server.Services.PlaceService.AllPlaceAdditions";

        private PlaceDao dao;

        public PlaceService(PlaceDao placeDao)
        {
            this.dao = placeDao;
        }

        public async Task<bool> AddPlace(Place p)
        {
            var result = await this.dao.InsertPlace(p);
            if (result)
            {
                // 删除缓存
                MemoryCacheHelper.Remove(_allPlacesCache);
                this.RemoveCache(p.Id);
            }

            return result;
        }

        public async Task<bool> UpdatePlace(Place p)
        {
            var result = await this.dao.UpdatePlace(p);
            if (result)
            {
                // 删除缓存
                MemoryCacheHelper.Remove(_allPlacesCache);
                this.RemoveCache(p.Id);
            }

            return result;
        }

        public async Task<bool> DeletePlace(long id)
        {
            var result = await this.dao.DeletePlace(id);
            if (result)
            {
                // 删除缓存
                MemoryCacheHelper.Remove(_allPlacesCache);
                this.RemoveCache(id);
            }

            return result;
        }

        public Task<List<Place>> GetAllPlaces() => MemoryCacheHelper.GetAsync(_allPlacesCache, () => this.dao.GetAllPlaces(), new TimeSpan(0, 30, 0));

        public async Task<Place> GetPlaceById(long id)
        {
            var places = await this.GetAllPlaces();
            return places?.FirstOrDefault(p => p.Id == id);
        }

        public async Task<bool> AddPlaceAddition(PlaceAddition p)
        {
            var result = await this.dao.InsertPlaceAddition(p);
            if (result)
            {
                // 删除缓存
                MemoryCacheHelper.Remove(_allPlaceAdditionsCache);
                this.RemoveCache(p.PlaceId);
            }

            return result;
        }

        public Task<List<PlaceAddition>> GetAllPlaceAdditions() => MemoryCacheHelper.GetAsync(_allPlaceAdditionsCache, 
            () => this.dao.GetAllPlaceAdditions(), new TimeSpan(0, 30, 0));

        public async Task<bool> UpdatePlaceAddition(PlaceAddition p)
        {
            var result = await this.dao.UpdatePlaceAddition(p);
            if (result)
            {
                // 删除缓存
                MemoryCacheHelper.Remove(_allPlaceAdditionsCache);
                this.RemoveCache(p.PlaceId);
            }

            return result;
        }

        public async Task<bool> DeletePlaceAddition(long id)
        {
            var addition = await this.GetPlaceAdditionById(id);
            if (addition == null)
            {
                return false;
            }

            var result = await this.dao.DeletePlaceAddition(id);
            if (result)
            {
                // 删除缓存
                MemoryCacheHelper.Remove(_allPlaceAdditionsCache);
                this.RemoveCache(addition.PlaceId);
            }

            return result;
        }


        public async Task<PlaceAddition> GetPlaceAdditionById(long id)
        {
            var additions = await this.GetAllPlaceAdditions();
            return additions?.FirstOrDefault(a => a.Id == id);
        }

        public async Task<PlaceDetail> GetPlaceDetail(long id)
        {
            var key = "V.TouristGuide.Server.Services.PlaceService.PlaceDetail." + id;
            return await MemoryCacheHelper.GetAsync(key, async () =>
            {
                var place = await this.GetPlaceById(id);
                if (place == null)
                {
                    return null;
                }

                var types = new List<string> { "article", "video", "comment" };
                if (place.Type == 0)
                {
                    types.Add("pic");
                }
                var additions = await this.dao.GetPlaceAdditions(id, types.ToArray());
                return new PlaceDetail
                {
                    Place = place,
                    Pictures = additions?.Where(a => a.Type == "pic").OrderByDescending(a => a.UpdateTime).ToList(),
                    Articles = additions?.Where(a => a.Type == "article").OrderByDescending(a => a.UpdateTime).ToList(),
                    Videos = additions?.Where(a => a.Type == "video").OrderByDescending(a => a.UpdateTime).ToList(),
                    Comments = additions?.Where(a => a.Type == "comment").OrderByDescending(a => a.UpdateTime).ToList()
                };
            }, new TimeSpan(0, 30, 0));
        }

        private void RemoveCache(long placeId)
        {
            MemoryCacheHelper.Remove("V.TouristGuide.Server.Services.PlaceService.PlaceDetail." + placeId);
        }
    }
}
