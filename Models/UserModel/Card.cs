using MongoDB.Bson;

namespace CardsServer.Models.UserModel;

public class Card
{
    public ObjectId Id { get; set; }
    public Name Name { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Image Image { get; set; }
    public Address Address { get; set; }
    public bool IsAdmin { get; set; } = false;
    public bool IsBussines { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
