using Orders.Shared.Responses;

namespace Orders.BackEnd.Helpers
{
    public interface IOrdersHelper
    {
        Task<ActionResponse<bool>> ProcessOrderAsync(string email, string remarks);
    }
}