// =====MARCO SECTION=====
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameScope;

public class GameData
{
    public string gameID;
    public string title;
    public Sprite Icon;
    public Sprite Logo;
    public string websiteURL;
    public string primaryGenre;
    public List<string> tags = new List<string>();
    public string ageRating;
    public float price;
    public string developer;
    public float criticScore;
    public float communityAverage;
    public int reviewCount;
    public int releaseYear;
}

public class UserData
{
    public List<string> likedGenres = new List<string>();
    public List<string> likedTags = new List<string>();
    public string maxAgeRating;
    public float preferredMaxPrice;
    public List<string> favouriteDevelopers = new List<string>();
    public List<GameRating> playHistory = new List<GameRating>();
    public Dictionary<string, int> ratings = new Dictionary<string, int>();
}

namespace GameScope.Scoring
{
    public static class ScoringBridge
    {
        public static GameData ToGameData(GameEntry g) => new GameData
        {
            gameID = g.Id.ToString(),
            title = g.Title,
            websiteURL = g.StoreUrl,
            primaryGenre = g.Genre,
            tags = g.Tags.ToList(),
            ageRating = g.Rating,
            price = g.Price,
            developer = g.Dev,
            criticScore = g.CriticScore,
            communityAverage = g.CommunityAvg,
            reviewCount = g.TotalRatings,
            releaseYear = g.Year,
        };

        public static UserData ToUserData(UserProfile u) => new UserData
        {
            likedGenres = u.LikedGenres,
            likedTags = u.LikedTags,
            maxAgeRating = u.MaxRating,
            preferredMaxPrice = u.PriceMax,
            favouriteDevelopers = u.FavCreators,
            playHistory = new List<GameRating>(),
            ratings = u.Ratings.ToDictionary(kv => kv.Key.ToString(), kv => kv.Value),
        };
    }
}

// =====END OF MARCO SECTION=====
