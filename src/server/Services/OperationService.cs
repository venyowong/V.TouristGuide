using V.TouristGuide.Server.Daos;
using V.TouristGuide.Server.Entities;
using V.TouristGuide.Server.Helpers;

namespace V.TouristGuide.Server.Services
{
    public class OperationService
    {
        private OperationDao dao;

        public OperationService(OperationDao dao)
        {
            this.dao = dao;
        }

        public async Task<bool> AddOperation(Operation operation)
        {
            var result = await this.dao.InsertOperation(operation);
            if (result)
            {
                // 删除缓存
                this.RemoveCache(operation);
            }

            return result;
        }

        public async Task<bool> UpdateOperation(long id, string remark)
        {
            var operation = await this.GetOperation(id);
            if (operation == null)
            {
                return false;
            }

            var result = await this.dao.UpdateOperation(operation.Id, operation.Remark);
            if (result)
            {
                // 删除缓存
                this.RemoveCache(operation);
            }

            return result;
        }

        public async Task<bool> DeleteOperation(long id)
        {
            var operation = await this.GetOperation(id);
            if (operation == null)
            {
                return false;
            }

            var result = await this.dao.DeleteOperation(id);
            if (result)
            {
                // 删除缓存
                this.RemoveCache(operation);
            }

            return result;
        }

        public async Task<Operation> GetOperation(long id)
        {
            var key = "V.TouristGuide.Server.Services.OperationService.Operation." + id;
            return await MemoryCacheHelper.GetAsync(key, () => this.dao.GetOperation(id), new TimeSpan(0, 5, 0));
        }

        public async Task<List<Operation>> GetPlaceOperations(long placeId)
        {
            var key = "V.TouristGuide.Server.Services.OperationService.Operations.Place." + placeId;
            return await MemoryCacheHelper.GetAsync(key, () => this.dao.GetPlaceOperations(placeId), new TimeSpan(0, 30, 0));
        }

        public async Task<List<Operation>> GetUserOperations(long userId)
        {
            var key = "V.TouristGuide.Server.Services.OperationService.Operations.User." + userId;
            return await MemoryCacheHelper.GetAsync(key, () => this.dao.GetUserOperations(userId), new TimeSpan(0, 30, 0));
        }

        private void RemoveCache(Operation operation)
        {
            MemoryCacheHelper.Remove("V.TouristGuide.Server.Services.OperationService.Operation." + operation.Id);
            MemoryCacheHelper.Remove("V.TouristGuide.Server.Services.OperationService.Operations.User." + operation.UserId);
            MemoryCacheHelper.Remove("V.TouristGuide.Server.Services.OperationService.Operations.Place." + operation.PlaceId);
        }
    }
}
