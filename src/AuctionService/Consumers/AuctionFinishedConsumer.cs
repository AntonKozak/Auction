using AuctionService.Data;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly AuctionDbContext _dbContext;
    public AuctionFinishedConsumer(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;

    }
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine("AuctionFinishedConsumer ---->>> Consuming auction finished event ---->>>");


        var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);

        if (auction == null)
        {
            return;
        }
        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = context.Message.Amount;
        }

        auction.Status = auction.SoldAmount > auction.ReservePrice
        ? Entities.Status.Finished : Entities.Status.ReserveNotMet;

        await _dbContext.SaveChangesAsync();
    }
}
