using CardsServer.Exceptions;
using CardsServer.Models.CardModel;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CardsServer.Services.Cards;

public class CardsService
{
    private readonly IMongoCollection<Models.CardModel.Card> _cards;
    public CardsService(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("dotnet_business_card_app");
        _cards = database.GetCollection<Models.CardModel.Card>("cards");
    }

    public async Task<Models.CardModel.Card> GetUserAsync(string cardId)
    {
        var filter = Builders<Models.CardModel.Card>.Filter.Eq(u => u.Id, new ObjectId(cardId));

        var card = await _cards.Find(filter).FirstOrDefaultAsync();

        return card;
    }

    public async Task<IEnumerable<Models.CardModel.Card>> GetAllCardAsync()
    {
        var cards = await _cards.Find(_ => true).ToListAsync();

        return cards;
    }

    public async Task<Card> CreateCardAsync(Card newCard)
    {
        var existingCard = await _cards.Find(c => c.Id == newCard.Id).FirstOrDefaultAsync();

        if (existingCard != null)
        {
            throw new CardAlreadyExistsException("Card with this id already exists.");
        }

        await _cards.InsertOneAsync(newCard);
        return newCard;
    }

    public async Task<bool> DeleteUserAsync(string cardId)
    {
        var filter = Builders<Card>.Filter.Eq(c => c.Id, new ObjectId(cardId));

        var result = await _cards.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }

    public async Task<bool> UpdateUserAsync(string cardId, Card updateCard)
    {
        try
        {
            var filter = Builders<Card>.Filter.Eq(u => u.Id, new ObjectId(cardId));

            var update = Builders<Card>.Update
                .Set(c => c.Email, updateCard.Email)
                .Set(c => c.Address, updateCard.Address)
                .Set(c => c.Phone, updateCard.Phone)
                .Set(c => c.Image, updateCard.Image);

            var results = await _cards.UpdateOneAsync(filter, update);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}
