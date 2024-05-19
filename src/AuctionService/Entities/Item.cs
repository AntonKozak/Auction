using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService.Entities;
// This attribute is used to specify the table name in the database
[Table("Items")]
public class Item
{
    public Guid Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public int Mileage { get; set; }
    public string ImageUrl { get; set; }

    // Navigation property
    public Auction Auction { get; set; }
    public Guid AuctionId { get; set; }
}
