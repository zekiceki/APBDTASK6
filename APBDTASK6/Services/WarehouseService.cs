using System.Threading.Tasks;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class WarehouseService : IWarehouseService
    
    {
        
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            
            _warehouseRepository = warehouseRepository ?? throw new ArgumentNullException(nameof(warehouseRepository));
        }

        public async Task<bool> CheckRequirementsAsync(Request request) {
            var amountGreater = _warehouseRepository.IsAmount(request);
            var productExists = await _warehouseRepository.ProductExistsAsync(request);
            var warehouseExists = await _warehouseRepository.WarehouseExistsAsync(request);
            var purchaseOrderExists = await _warehouseRepository.PurchaseOrderExistsAsync(request);

            return productExists && warehouseExists && purchaseOrderExists && amountGreater;
        }

        public async Task<bool> IsOrderCompletedAsync(Request request) {
            return await _warehouseRepository.IsOrderCompletedAsync(request);
        }

        public async Task<int> MainScenarioAsync(Request request) {
            if (await CheckRequirementsAsync(request) && await IsOrderCompletedAsync(request))
            {
                if (await _warehouseRepository.UpdateOrderDateAsync(request)) {
                    return await _warehouseRepository.InsertRecordAsync(request);
                }
            }

            return 0;
        }

        public async Task<int> SecondScenarioAsync(Request request) {
            return await _warehouseRepository.ProductStoredExistsAsync(request);
        }
    }
}