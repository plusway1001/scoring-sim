using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData : MonoBehaviour
{
    public string gameID;
    public string title;

    public string primaryGenre;
    public List<string> tags = new();

    public string ageRating;

    public float price;

    public int releaseYear;

    // General Score Inputs

    public float criticScore;          //0-100

    public float communityAverage;     //0-10

    public int reviewCount;

    public string developer;

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
