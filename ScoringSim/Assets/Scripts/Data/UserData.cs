using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData : MonoBehaviour
{
    public string username;

    public List<string> likedGenres = new();

    //public List<string> dislikedGenres = new();

    public List<string> likedTags = new();

    public string maxAgeRating;

    //public float preferredMinPrice;

    public float preferredMaxPrice;

    [Header("Activity")]

    public List<GameData> wishlist = new();

    //public List<GameData> completedGames = new();

    public List<string> favouriteDevelopers = new();

    // Developers the user has rated 8+ before
    public List<GameRating> playHistory = new(); // A Dynamic Design System

    // GameID -> Rating (1-10)

    public Dictionary<string, int> ratings = new();

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
