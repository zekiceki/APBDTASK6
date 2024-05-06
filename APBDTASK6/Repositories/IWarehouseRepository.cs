using System.Threading.Tasks;
using WebApplication1;

public interface IWarehouseRepository
{
    // Check if product exists
    Task<bool> ProductExistsAsync(Request request);

    // Check if warehouse exists
    Task<bool> WarehouseExistsAsync(Request request);

    // Check if purchase order exists
    Task<bool> PurchaseOrderExistsAsync(Request request);

    // Check if order is completed
    Task<bool> IsOrderCompletedAsync(Request request);

    // Update order date
    Task<bool> UpdateOrderDateAsync(Request request);

    // Insert a record
    Task<int> InsertRecordAsync(Request request);

    // Get the last inserted ID
    Task<int> GetLastIdAsync();

    // Check if the amount is valid
    bool IsAmount(Request request);

    // Get order ID
    Task<int> GetOrderIdAsync(Request request);

    // Check if product is stored
    Task<int> ProductStoredExistsAsync(Request request);
}