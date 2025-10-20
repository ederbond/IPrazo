using Refit;

namespace IPrazo.Crowler;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Iniciando...");

        var api = RestService.For<IApi>("https://proxyservers.pro");

        for (int i = 1; i <= 8; i++)
        {
            var response = await api.GetPageDataAsync(1);

            var html = await response.Content.ReadAsStringAsync();

            // Parse table and extract data
            var data = ProxyParser.ParseProxyTable(html);

            Console.WriteLine($"Encontramos {data.Count} registros.");
            foreach (var p in data)
            {
                Console.WriteLine(p);
            }
        }

        Console.ReadKey();
    }
}
