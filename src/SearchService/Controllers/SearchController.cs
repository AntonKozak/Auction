using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
    {
        var query = DB.PagedSearch<Item, Item>();

        //{{searchApi}}/api/search?searchTerm= giving all the items when wuery is empty
        // {{searchApi}}/api/search giving all the items
        if (!string.IsNullOrWhiteSpace(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        //{{searchApi}}/api/search?searchTerm=ford 
        query = searchParams.OrderBy switch
        {
            "make" => query.Sort(x => x.Ascending(a => a.Make)),
            "model" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
        };

        //{{searchApi}}/api/search?filterBy=finished
        query = searchParams.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6)
             && x.AuctionEnd > DateTime.UtcNow),
            _ => query
        };

        //{{searchApi}}/api/search?seller=tom
        if (!string.IsNullOrWhiteSpace(searchParams.Seller))
        {
            query.Match(x => x.Seller == searchParams.Seller);
        }

        //{{searchApi}}/api/search?winner=alice
        if (!string.IsNullOrWhiteSpace(searchParams.Winner))
        {
            query.Match(x => x.Winner == searchParams.Winner);
        }

        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);



        var result = await query.ExecuteAsync();

        return Ok(new
        {
            /*returns the 
            *result = 4 items which was asked, 
            *page count = how many pages of items total 
            *total count = total items in the database
            */
            result = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });


    }
}