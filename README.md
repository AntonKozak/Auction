# Auction-house

Microservices based app using .Net, NextJS, IdentityServer, RabbitMQ running on Docker and Kubernetes<br>

Creating solution file<br>
1 dotnet new sln -n Auction-house<br>
Creating Web API project with -o for output directory and --use-controllers for using controllers instead of endpoints<br>
2 dotnet new webapi -o src/Auction-house --use-controllers<br>
Put directory in the solution file<br>
dotnet sln add src/AuctionService<br>

Give inforamtion about the request(query) and response
"Microsoft.AspNetCore": "Information

# Github

Some information about the github repository and how to use it.<br>

Show all branches and commits<br>
git log --all --decorate --onlaine --graph<br>

Show changes in a branch<br>
git checkout -b "branch_name"<br>

Create a new branch<br>
git branch "branch_name"<br>

Show all branches<br>
git branch<br>

# Create an alias for a command :

git config --global alias."alias_name" "command"<br>
Exempel:<br>
Making global command for log --all --decorate --oneline --graph<br>
git config --global alias.lgb "log --all --decorate --oneline --graph"<br>
We can now use the command by typing<br>
git lgb<br>

# Auction-house Architecture

![Auction](Auction-house_Architecture.png)

# Infrastructure

.Net Web API<br>
Postgres DB<br>
Entity Framework ORM<br>
Service Bus - RabbitMQ<br>

Nuget Packages<br>
AutoMapper.Extensions.Microsoft.DependancyInjection<br>
Microsoft.AspNetCore.Authentication.JwtBearer<br>
Microsoft.EntityFrameworkCore.Design<br>
Npgsql.EntityFrameworkCore.PostgreSQL<br>
MassTransit.RabbitMQ<br>

Queries handled<br>
ById - Get auction by Id<br>
All - Get all auctions<br>
ByStatus - Get auctions by status<br>
BySeller - Get auctions by seller<br>
ByBuyer - Get auctions by buyer<br>

Events emitted<br>
AuctionCreated - When the auction is created, in response to CreateAuction -<br>
Emits AuctionDto<br>

Events consumed<br>
BidService.BidPlaced - When a bid has been placed in the BidService<br>
BidService.BiddingFinished - When an auction has reached the AuctionEnd date.<br>

API Endpoints<br>
Models<br>
DTOs<br>

## Docker

Build the image<br>
docker compose up -d <br>
