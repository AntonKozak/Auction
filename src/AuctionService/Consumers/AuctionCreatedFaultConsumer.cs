using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine("--> --> --> Consuming faulty creation--> --> --> --> ");

        var exeption = context.Message.Exceptions.First();

        if (exeption.ExceptionType == $"{exeption.ExceptionType}")
        {
            context.Message.Message.Model = "FooBarTesting";
            await context.Publish(context.Message.Message);
        }
        else
        {
            Console.WriteLine("Not an argument exception: updating error dashboard somewhere...");
        }


    }
}
