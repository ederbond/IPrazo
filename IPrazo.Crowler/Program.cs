// See https://aka.ms/new-console-template for more information
using IPrazo.Crowler;
using Refit;

Console.WriteLine("Iniciando...");

var api = RestService.For<IApi>("https://proxyservers.pro");

var response = await api.GetPageDataAsync(1);

var content = await response.Content.ReadAsStringAsync();


Console.ReadKey();
