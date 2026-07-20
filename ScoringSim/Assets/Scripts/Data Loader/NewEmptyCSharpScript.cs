using System;
using System.Collections.Generic;

[Serializable]
public class UserJson
{
    public string username;

    public List<string> likedGenres;

    public List<string> likedTags;

    public string maxAgeRating;

    public float preferredMaxPrice;

    public List<string> favouriteDevelopers;
}

[Serializable]
public class GameJson
{
    public string gameID;

    public string title;

    public string iconPath;

    public string logoPath;

    public string websiteURL;

    public string primaryGenre;

    public List<string> tags;

    public string ageRating;

    public float price;

    public int releaseYear;

    public float criticScore;

    public List<int> userRatings;

    public int reviewCount;

    public string developer;
}