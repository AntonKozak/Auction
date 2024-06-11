using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Entities;

namespace SearchService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        Console.WriteLine($"Bid placed: {context.Message.AuctionId}");

        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

        if (context.Message.BidStatus.Contains("Accepted")
         && context.Message.Amount > auction.CurrentHighBid)
        {
            auction.CurrentHighBid = context.Message.Amount;
            await auction.SaveAsync();
        }
    }
}
