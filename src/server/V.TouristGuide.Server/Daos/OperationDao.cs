using Dapper;
using System.Data;
using V.TouristGuide.Server.Entities;

namespace V.TouristGuide.Server.Daos
{
    public class OperationDao
    {
        private IDbConnection conn;

        public OperationDao(IDbConnection conn)
        {
            this.conn = conn;
        }

        public async Task<bool> InsertOperation(Operation operation)
        {
            var result = await this.conn.ExecuteAsync(@"INSERT INTO public.operation(user_id, place_id, type, remark)
	            VALUES (@UserId, @PlaceId, @Type, @Remark);", operation);
            return result > 0;
        }

        public async Task<bool> UpdateOperation(long id, string remark)
        {
            var result = await this.conn.ExecuteAsync("UPDATE public.operation SET remark=@remark, update_time=NOW() WHERE id=@id;", new { id, remark });
            return result > 0;
        }

        public async Task<bool> DeleteOperation(long id)
        {
            var result = await this.conn.ExecuteAsync($"UPDATE public.operation SET is_valid=false, update_time=NOW() WHERE id={id};");
            return result > 0;
        }

        public async Task<List<Operation>> GetPlaceOperations(long placeId)
        {
            var result = await this.conn.QueryAsync<Operation>($"SELECT * FROM public.operation WHERE place_id={placeId} AND is_valid=true;");
            return result.ToList();
        }

        public async Task<List<Operation>> GetUserOperations(long userId)
        {
            var result = await this.conn.QueryAsync<Operation>($"SELECT * FROM public.operation WHERE user_id={userId} AND is_valid=true;");
            return result.ToList();
        }

        public async Task<Operation> GetOperation(long id)
        {
            return await this.conn.QueryFirstOrDefaultAsync<Operation>($"SELECT * FROM public.operation WHERE id={id};");
        }
    }
}
