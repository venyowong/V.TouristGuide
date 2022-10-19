using Dapper;
using Serilog;
using SqlKata.Execution;
using System.Data;
using System.Net;
using System.Xml.Linq;
using V.TouristGuide.Server.Entities;

namespace V.TouristGuide.Server.Daos
{
    public class PlaceDao
    {
        private QueryFactory db;

        public PlaceDao(QueryFactory db)
        {
            this.db = db;
        }

        public async Task<bool> InsertPlace(Place p)
        {
            var result = await this.db.Query("place")
                .InsertAsync(new
                {
                    name = p.Name,
                    cover = p.Cover,
                    open_time = p.OpenTime,
                    address = p.Address,
                    lng = p.Lng,
                    lat = p.Lat,
                    tel = p.Tel,
                    type = p.Type,
                    visibility = p.Visibility,
                    region = p.Region
                });
            return result > 0;
        }

        public async Task<bool> UpdatePlace(Place p)
        {
            var result = await this.db.Query("place")
                .Where("id", p.Id)
                .UpdateAsync(new
                {
                    name = p.Name,
                    cover = p.Cover,
                    open_time = p.OpenTime,
                    address = p.Address,
                    lng = p.Lng,
                    lat = p.Lat,
                    tel = p.Tel,
                    type = p.Type,
                    visibility = p.Visibility,
                    update_time = DateTime.Now
                });
            return result > 0;
        }

        public async Task<bool> DeletePlace(long id)
        {
            var result = await this.db.Query("place")
                .Where("id", id)
                .UpdateAsync(new
                {
                    is_valid = false,
                    update_time = DateTime.Now
                });
            return result > 0;
        }

        public async Task<List<Place>> GetAllPlaces()
        {
            var result = await this.db.Query("place")
                .WhereTrue("is_valid")
                .GetAsync<Place>();
            return result?.ToList();
        }

        public async Task<bool> InsertPlaceAddition(PlaceAddition p)
        {
            var result = await this.db.Query("place_addition")
                .InsertAsync(new
                {
                    place_id = p.PlaceId,
                    type = p.Type,
                    title = p.Title,
                    img = p.Img,
                    url = p.Url,
                    desc = p.Desc,
                    source = p.Source,
                    source_url = p.SourceUrl
                });
            return result > 0;
        }

        public async Task<List<PlaceAddition>> GetAllPlaceAdditions()
        {
            var result = await this.db.Query("place_addition")
                .WhereTrue("is_valid")
                .GetAsync<PlaceAddition>();
            return result?.ToList();
        }

        public async Task<bool> UpdatePlaceAddition(PlaceAddition p)
        {
            var result = await this.db.Query("place_addition")
                .Where("id", p.Id)
                .UpdateAsync(new
                {
                    place_id = p.PlaceId,
                    title = p.Title,
                    img = p.Img,
                    desc = p.Desc,
                    source = p.Source,
                    source_url = p.SourceUrl,
                    type = p.Type,
                    url = p.Url,
                    update_time = DateTime.Now
                });
            return result > 0;
        }

        public async Task<bool> DeletePlaceAddition(long id)
        {
            var result = await this.db.Query("place_addition")
                .Where("id", id)
                .UpdateAsync(new
                {
                    is_valid = false,
                    update_time = DateTime.Now
                });
            return result > 0;
        }

        public Task<Place> GetPlace(long id) => this.db.Query("place")
            .Where("id", id)
            .WhereTrue("is_valid")
            .FirstAsync<Place>();

        public async Task<List<PlaceAddition>> GetPlaceAdditions(long placeId, params string[] types)
        {
            var result = await this.db.Query("place_addition")
                .Where("place_id", placeId)
                .WhereIn("type", types)
                .WhereTrue("is_valid")
                .GetAsync<PlaceAddition>();
            return result?.ToList();
        }
    }
}
