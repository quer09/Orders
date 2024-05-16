namespace Orders.FrontEnd.Repositories
{
    public interface IRepository
    {
        Task<HttpResponseWrapper<T>> GetAsync<T>(string url);

        Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model);

        Task<HttpResponseWrapper<TActionResponce>> PostAsync<T, TActionResponce>(string url, T model);
    }
}