using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Entities;
using SearchService;
using SearchService.Services;

namespace SearchService.Data;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        try
        {
            var connectionString = app.Configuration.GetConnectionString("MongoDbConnection");
            await DB.InitAsync("SearchDb", MongoClientSettings.FromConnectionString(connectionString));

            await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Year, KeyType.Text)
                .CreateAsync();

            var count = await DB.CountAsync<Item>();

            if (count == 0)
            {
                using var scope = app.Services.CreateScope();
                var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();
                var items = await httpClient.GetItemsForSearchDb();

                Console.WriteLine($"Items from AuctionService: {items.Count}");

                if (items.Count > 0)
                {
                    await DB.SaveAsync(items);
                }

                Console.WriteLine($"Items in SearchDb: {await DB.CountAsync<Item>()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while initializing the database: {ex.Message}");
        }
    }

}
