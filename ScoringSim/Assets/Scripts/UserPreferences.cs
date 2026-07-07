using System.Collections.Generic;

[System.Serializable]
public class UserPreferences
{
    public List<string> likedGenres = new List<string>();

    public List<string> likedTags = new List<string>();

    public List<string> favouriteCreators = new List<string>();

    public string maxAgeRating;

    public float maxPrice;
}
