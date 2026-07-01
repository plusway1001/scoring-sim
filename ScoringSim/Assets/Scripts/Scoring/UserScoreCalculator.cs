using UnityEngine;

public static class UserScoreCalculator
{
    public static ScoreBreakdown Calculate(GameData game, UserData user)
    {
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

        if (user.dislikedGenres.Contains(game.primaryGenre))
            result.genreModifier -= 20;

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
            result.finalScore = 0;
            return result;
        }

        //----------------------------------
        // Price

        if (game.price >= user.preferredMinPrice &&
           game.price <= user.preferredMaxPrice)
        {
            result.priceModifier = 5;
        }

        //----------------------------------
        // Developer

        if (user.favouriteDevelopers.Contains(game.developer))
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
}