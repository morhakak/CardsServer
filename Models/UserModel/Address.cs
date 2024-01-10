namespace CardsServer.Models.UserModel;

public class Address
{
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public int HouseNumber { get; set; }
    public int Zip { get; set; } = 0;
}
