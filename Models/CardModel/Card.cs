using CardsServer.Models.UserModel;
using MongoDB.Bson;

namespace CardsServer.Models.CardModel;

public class Card
{
    public ObjectId Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Subtitle { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Web { get; set; } = string.Empty;

    public Image Image { get; set; }
    public Address Address { get; set; }

    public int BizNumber { get; set; }

    public List<string> Likes { get; set; } = [];

    public DateTime CreateAt { get; set; } = DateTime.Now;

    public string UserId { get; set; } = string.Empty;
}
