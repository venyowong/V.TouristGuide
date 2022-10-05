using Dapper;
using System.Data;
using V.TouristGuide.Server.Entities;

namespace V.TouristGuide.Server.Daos
{
    public class PlaceDao
    {
        private IDbConnection conn;

        public PlaceDao(IDbConnection connection)
        {
            this.conn = connection;
        }

        public async Task<bool> InsertPlace(Place p)
        {
            var result = await this.conn.ExecuteAsync(@"INSERT INTO place(name, cover, open_time, address, lng, lat, tel, type, visibility, region) 
                VALUES(@Name, @Cover, @OpenTime, @Address, @Lng, @Lat, @Tel, @Type, @Visibility, @Region)", p);
            return result > 0;
        }

        public async Task<bool> UpdatePlace(Place p)
        {
            var result = await this.conn.ExecuteAsync($@"UPDATE place SET name=@Name, cover=@Cover, open_time=@OpenTime,
                address=@Address, lng=@Lng, lat=@Lat, tel=@Tel, type=@Type, visibility=@Visibility, update_time=NOW() WHERE id=@Id", p);
            return result > 0;
        }

        public async Task<bool> DeletePlace(long id)
        {
            var result = await this.conn.ExecuteAsync($"UPDATE place SET is_valid=false, update_time=NOW() WHERE id={id}");
            return result > 0;
        }

        public async Task<List<Place>> GetAllPlaces()
        {
            var result = await this.conn.QueryAsync<Place>("SELECT * FROM place WHERE is_valid=true");
            return result?.ToList();
        }

        public async Task<bool> InsertPlaceAddition(PlaceAddition p)
        {
            var result = await this.conn.ExecuteAsync(@"INSERT INTO place_addition(place_id, type, title, img, url, ""desc"", source, source_url) 
                VALUES(@PlaceId, @Type, @Title, @Img, @Url, @Desc, @Source, @SourceUrl)", p);
            return result > 0;
        }

        public async Task<List<PlaceAddition>> GetAllPlaceAdditions()
        {
            var result = await this.conn.QueryAsync<PlaceAddition>("SELECT * FROM place_addition WHERE is_valid=true");
            return result?.ToList();
        }

        public async Task<bool> UpdatePlaceAddition(PlaceAddition p)
        {
            var result = await this.conn.ExecuteAsync($@"UPDATE place_addition SET place_id=@PlaceId, title=@Title, img=@Img,
                ""desc""=@Desc, source=@Source, source_url=@SourceUrl, type=@Type, url=@Url, update_time=NOW() WHERE id=@Id", p);
            return result > 0;
        }

        public async Task<bool> DeletePlaceAddition(long id)
        {
            var result = await this.conn.ExecuteAsync($"UPDATE place_addition SET is_valid=false, update_time=NOW() WHERE id={id}");
            return result > 0;
        }

        public Task<Place> GetPlace(long id) => this.conn.QueryFirstOrDefaultAsync<Place>($"SELECT * FROM place WHERE id={id} AND is_valid=true");

        public async Task<List<PlaceAddition>> GetPlaceAdditions(long placeId, params string[] types)
        {
            var result = await this.conn.QueryAsync<PlaceAddition>($"SELECT * FROM place_addition WHERE place_id={placeId} AND type=ANY(@Type) AND is_valid=true", new { Type = types });
            return result?.ToList();
        }
    }
}
