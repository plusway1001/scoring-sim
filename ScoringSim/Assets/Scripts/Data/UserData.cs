using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData : MonoBehaviour
{
    public string username;

    public List<string> likedGenres = new();

    public List<string> dislikedGenres = new();

    public List<string> likedTags = new();

    public string maxAgeRating;

    public float preferredMinPrice;

    public float preferredMaxPrice;

    public List<string> favouriteDevelopers = new();

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
