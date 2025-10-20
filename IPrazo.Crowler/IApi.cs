using Refit;

namespace IPrazo.Crowler;

public interface IApi
{
    [Get("/proxy/list/order/updated/order_dir/desc/page/{pageNumber}")]
    Task<HttpResponseMessage> GetPageDataAsync(int pageNumber);
}
