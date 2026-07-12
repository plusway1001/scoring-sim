using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameGenreData
{
    public string gameID;
    public string title;

    public List<string> genres = new List<string>();
    public List<string> tags = new List<string>();

    public string ageRating;
    public float price;
    public int releaseYear;
    public string developer;

    public float criticScore;
    public float communityRatingAverage;
    public float generalScore;
}