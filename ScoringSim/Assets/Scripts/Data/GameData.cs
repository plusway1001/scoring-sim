using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class GameData : MonoBehaviour
{
    public string gameID;
    public string title;
    public Sprite Icon;
    public Sprite Logo;
    public string websiteURL;

    public string primaryGenre;
    public List<string> tags = new();
    private List<string> genres = new();

    public string ageRating;

    public float price;

    public int releaseYear;

    // General Score Inputs

    public float criticScore;          //0-100

    public float communityAverage;     //0-10

    public List<int> userRatings = new(); //0-10

    public int reviewCount;

    public string developer;

    public int yourRatings;

    GenreTagDataLoader tagDataLoader;

    private void Start()
    {
        GenerateUserRating();
        tagDataLoader = FindAnyObjectByType<GenreTagDataLoader>();

        LoadGenreCSV();
    }

    public void LoadGenreCSV()
    {
        // assign the genres from the data loader
        foreach (var game in tagDataLoader.games)
        {
            if (title == game.title)
            {
                game.genres = genres;
            }
        }
    }

    public float CalculateAverage(List<int> values)
    {
        if (values == null || values.Count == 0)
            return 0f;

        int sum = 0;

        foreach (int value in values)
        {
            sum += value;
        }

        return (float)sum / values.Count;
    }

    public void UpdateNewUserRating(int rating)
    {
        userRatings.Add(rating);
        communityAverage = CalculateAverage(userRatings);
        Debug.Log(communityAverage);
    }

    public float GenerateUserRating()
    {
        communityAverage = CalculateAverage(userRatings);
        Debug.Log(communityAverage);
        return communityAverage;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ScoreDisplayUI ui = FindFirstObjectByType<ScoreDisplayUI>();

        if (ui != null)
        {
            ui.UpdateUI();
        }
    }
#endif
}
