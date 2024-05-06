using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApplication1;

namespace WebApplication1.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        
        private readonly string _connectionString;

        public WarehouseRepository(string connectionString)
        {
            
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
        

        private async Task<SqlConnection> OpenConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
        

        private async Task<int> ExecuteScalarAsync(SqlCommand command)
        {
            
            using (var connection = await OpenConnectionAsync())
            {
                
                command.Connection = connection;
                
                return (int)await command.ExecuteScalarAsync();
            }
            
        }

        public async Task<bool> ProductExistsAsync(Request request)
        {
            using var command = new SqlCommand("SELECT COUNT(*) FROM Product WHERE IdProduct = @ProductId");
            command.Parameters.AddWithValue("@ProductId", request.IdProduct);
            return await ExecuteScalarAsync(command) > 0;
        }

        public async Task<bool> WarehouseExistsAsync(Request request) {
            using var command = new SqlCommand("SELECT COUNT(*) FROM Warehouse WHERE IdWarehouse = @WarehouseId");
            command.Parameters.AddWithValue("@WarehouseId", request.IdWarehouse);
            return await ExecuteScalarAsync(command) > 0;
        }

        public async Task<bool> PurchaseOrderExistsAsync(Request request) {
            using var command = new SqlCommand("SELECT COUNT(*) FROM [Order] WHERE IdProduct = @productId AND Amount = @Amount AND CreatedAt < @createdAt");
            command.Parameters.AddWithValue("@Amount", request.Amount);
            command.Parameters.AddWithValue("@productId", request.IdProduct);
            command.Parameters.AddWithValue("@createdAt", request.CreatedAt);
            return await ExecuteScalarAsync(command) > 0;
        }

        public async Task<bool> IsOrderCompletedAsync(Request request) {
            using var command = new SqlCommand("SELECT COUNT(*) FROM Product_Warehouse WHERE IdOrder = @orderId");
            
            command.Parameters.AddWithValue("@orderId", await GetOrderIdAsync(request));
            
            return await ExecuteScalarAsync(command) == 0;
        }

        public async Task<bool> UpdateOrderDateAsync(Request request) {
            using var command = new SqlCommand("UPDATE [Order] SET FulfilledAt = @currentDate WHERE IdOrder = @orderId");
            
            command.Parameters.AddWithValue("@orderId", await GetOrderIdAsync(request));
            
            command.Parameters.AddWithValue("@currentDate", DateTime.Now);
            return await ExecuteScalarAsync(command) > 0;
        }

        public async Task<int> InsertRecordAsync(Request request)
        {
            var lastId = await GetLastIdAsync();
            var newId = lastId + 1;

            using (var connection = await OpenConnectionAsync())
            using (var command = new SqlCommand("INSERT INTO Product_Warehouse(IdProductWarehouse, IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES (@IdProductWarehouse, @IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @Date)", connection))
            {
                command.Parameters.AddWithValue("@IdProductWarehouse", newId);
                
                command.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
                
                command.Parameters.AddWithValue("@IdProduct", request.IdProduct);
                
                command.Parameters.AddWithValue("@IdOrder", await GetOrderIdAsync(request));
                
                command.Parameters.AddWithValue("@Amount", request.Amount);
                
                command.Parameters.AddWithValue("@Date", DateTime.Now);

                await command.ExecuteNonQueryAsync();

                return newId;
            }
        }

        public async Task<int> GetLastIdAsync()
        {
            using var command = new SqlCommand("SELECT IdProductWarehouse FROM Product_Warehouse ORDER BY IdProductWarehouse DESC");
            return await ExecuteScalarAsync(command);
        }

        public async Task<int> GetOrderIdAsync(Request request) {
            using var command = new SqlCommand("SELECT IdOrder FROM [Order] WHERE CreatedAt = @createdAt");
            command.Parameters.AddWithValue("@createdAt", request.CreatedAt);
            return await ExecuteScalarAsync(command);
        }

        public async Task<int> ProductStoredExistsAsync(Request request) {
            using var connection = await OpenConnectionAsync();
            using var command = new SqlCommand("AddProductToWarehouse", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@IdProduct", request.IdProduct);
            
            command.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
            
            command.Parameters.AddWithValue("@Amount", request.Amount);
            
            command.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);
            
            return (int)await command.ExecuteScalarAsync();
        }

        public bool IsAmount(Request request) {
            return request.Amount > 0;
        }
    }
}
