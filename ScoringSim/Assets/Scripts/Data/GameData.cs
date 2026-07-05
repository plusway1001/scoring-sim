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

    public string ageRating;

    public float price;

    public int releaseYear;

    // General Score Inputs

    public float criticScore;          //0-100

    public float communityAverage;     //0-10

    public List<int> userRatings = new(); //0-10

    public int reviewCount;

    public string developer;

    private void Start()
    {
        GenerateUserRating();
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

    public void GenerateUserRating()
    {
        communityAverage = CalculateAverage(userRatings);
        Debug.Log(communityAverage);
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
