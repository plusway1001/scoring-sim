// NEW: converts VideoScope's GameEntry/UserProfile catalogue objects into the
// plain GameData/UserData shape main2's GeneralScoreCalculator/UserScoreCalculator
// expect, so those files run completely unmodified against the real catalogue.
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameScope;

// NOTE: GameData/UserData are deliberately in the GLOBAL namespace (not
// GameScope.Scoring) because main2's GeneralScoreCalculator.cs and
// UserScoreCalculator.cs (dropped in unmodified, also global-namespace) refer to
// `GameData`/`UserData` unqualified. Keeping these two types global is what lets
// that file compile completely untouched.
//
// This GameData is a superset: besides the fields GeneralScoreCalculator/
// UserScoreCalculator read, it also carries title/Icon/Logo/websiteURL, because
// Scripts/UI_Display_Test/ScoreDisplayUI.cs and GameDatabase.cs (also already
// dropped into this project, unwired, from main2's separate test-harness scene)
// reference a `GameData` with those fields too. One shared type keeps everything
// that expects "GameData" compiling against the same definition instead of
// splitting into competing types.

/// <summary>Field-compatible stand-in for main2's Data/GameData.cs (see the
/// GameScope.Legacy-namespaced original in Data/GameData.cs), holding every field
/// that either the Scoring calculators or ScoreDisplayUI.cs/GameDatabase.cs read.</summary>
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

/// <summary>Field-compatible stand-in for main2's Data/UserData.cs, holding only
/// what GeneralScoreCalculator/UserScoreCalculator read.</summary>
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
            // GameScope's UserProfile doesn't track a separate play-history list
            // (main2's own UserScoreCalculator only consults it when the
            // `hasPlayHistory` flag near the top of Calculate() is flipped to
            // true, which it isn't by default) — an empty list preserves main2's
            // current, actual behaviour exactly.
            playHistory = new List<GameRating>(),
            ratings = u.Ratings.ToDictionary(kv => kv.Key.ToString(), kv => kv.Value),
        };
    }
}
