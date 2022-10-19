using Dapper;
using SqlKata.Execution;
using System.Data;
using V.TouristGuide.Server.Entities;

namespace V.TouristGuide.Server.Daos
{
    public class OperationDao
    {
        private QueryFactory db;

        public OperationDao(QueryFactory db)
        {
            this.db = db;
        }

        public async Task<bool> InsertOperation(Operation operation)
        {
            var result = await this.db.Query("operation")
                .InsertAsync(new
                {
                    user_id = operation.UserId,
                    place_id = operation.PlaceId,
                    type = operation.Type,
                    remark = operation.Remark
                });
            return result > 0;
        }

        public async Task<bool> UpdateOperation(long id, string remark)
        {
            var result = await this.db.Query("operation")
                .Where("id", id)
                .UpdateAsync(new
                {
                    update_time = DateTime.Now,
                    remark = remark
                });
            return result > 0;
        }

        public async Task<bool> DeleteOperation(long id)
        {
            var result = await this.db.Query("operation")
                .Where("id", id)
                .UpdateAsync(new
                {
                    is_valid = false,
                    update_time = DateTime.Now
                });
            return result > 0;
        }

        public async Task<List<Operation>> GetPlaceOperations(long placeId)
        {
            var result = await this.db.Query("operation")
                .Where("place_id", placeId)
                .WhereTrue("is_valid")
                .GetAsync<Operation>();
            return result.ToList();
        }

        public async Task<List<Operation>> GetUserOperations(long userId)
        {
            var result = await this.db.Query("operation")
                .Where("user_id", userId)
                .WhereTrue("is_valid")
                .GetAsync<Operation>();
            return result.ToList();
        }

        public async Task<Operation> GetOperation(long id)
        {
            return await this.db.Query("operation")
                .Where("id", id)
                .FirstAsync<Operation>();
        }
    }
}
