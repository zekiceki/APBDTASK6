
using WebApplication1;

public interface IWarehouseService
{
    public  Task<bool> CheckRequirementsAsync(Request request);

    public Task<bool> IsOrderCompletedAsync(Request request);

    public Task<int> MainScenarioAsync(Request request);

    public Task<int> SecondScenarioAsync(Request request);
}