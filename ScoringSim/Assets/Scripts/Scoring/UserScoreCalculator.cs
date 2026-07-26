// === CODE FROM main2/Assets/Scripts/Scoring/UserScoreCalculator.cs === LINE 1-130 ===
// Verbatim from the team's main2 branch — no logic changes (including the two
// commented-out lines: disliked-genre penalty and the price-minimum check are
// exactly as disabled in main2, not something removed during this merge), except
// the one Debug.Log below is also commented out to cut console spam.
// Operates on `GameData`/`UserData`; see Scripts/Scoring/ScoringBridge.cs.
using UnityEngine;

public static class UserScoreCalculator
{

    public static ScoreBreakdown Calculate(GameData game, UserData user)
    {
        bool hasPlayHistory = false; // Set this to true if you want to have play history over fav creator/developers.

        ScoreBreakdown result = new();

        float generalScore = GeneralScoreCalculator.Calculate(game);

        result.generalScore = generalScore;

        //----------------------------------
        // Self Rating Override
        //----------------------------------

        if (user.ratings.ContainsKey(game.gameID))
        {
            result.selfRated = true;
            result.finalScore = user.ratings[game.gameID] * 10;
            return result;
        }

        //----------------------------------
        // Genre
        //----------------------------------

        if (user.likedGenres.Contains(game.primaryGenre))
            result.genreModifier += 20;

        /*if (user.dislikedGenres.Contains(game.primaryGenre))
            result.genreModifier -= 20;*/

        //----------------------------------
        // Tags
        //----------------------------------

        foreach (string tag in game.tags)
        {
            if (user.likedTags.Contains(tag))
                result.tagModifier += 2;
        }

        result.tagModifier = Mathf.Clamp(result.tagModifier, 0, 10);

        //----------------------------------
        // Age

        if (IsAgeRestricted(game.ageRating, user.maxAgeRating))
        {
            result.ageRestricted = true;
            // Debug.Log("Test Max Age!");
            result.finalScore = 0;
            return result;
        }

        //----------------------------------
        // Price

        if (game.price >= 0 &&
           game.price <= user.preferredMaxPrice)
        {
            result.priceModifier = 5;
        }

        /*if (game.price >= user.preferredMinPrice &&
           game.price <= user.preferredMaxPrice)
        {
            result.priceModifier = 5;
        }*/

        //----------------------------------
        // Developer

        if (HasDeveloperHistory(game, user) && hasPlayHistory) // A Dynamic System Design on top of Fav Creator/Developer
        {
            result.developerModifier = 10;
        }

        if (user.favouriteDevelopers.Contains(game.developer) && !hasPlayHistory) // By Default - No Play History, only fav creator/developer
        {
            result.developerModifier = 10;
        }

        //----------------------------------

        result.finalScore =
            generalScore +
            result.genreModifier +
            result.tagModifier +
            result.priceModifier +
            result.developerModifier;

        result.finalScore = Mathf.Clamp(result.finalScore, 0, 100);

        return result;
    }

    static bool IsAgeRestricted(string game, string user)
    {
        string[] ratings = { "G", "PG13", "R16", "R18" };

        return System.Array.IndexOf(ratings, game)
             > System.Array.IndexOf(ratings, user);
    }

    private static bool HasDeveloperHistory(GameData game, UserData user)
    {
        foreach (GameRating playedGame in user.playHistory)
        {
            if (playedGame.developer == game.developer &&
               playedGame.rating >= 8)
            {
                return true;
            }
        }

        return false;
    }
}