using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private GameDatabase database;

    public void LoadGames()
    {
        database.games.Clear();

        // Load every json inside Resources/Games
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("Games");

        foreach (TextAsset json in jsonFiles)
        {
            GameJson data = JsonUtility.FromJson<GameJson>(json.text);

            // Create a new GameObject
            GameObject obj = new GameObject(data.gameID);

            // Add the GameData component
            GameData game = obj.AddComponent<GameData>();

            // Fill in the data
            game.gameID = data.gameID;
            game.title = data.title;

            game.Icon = Resources.Load<Sprite>(data.iconPath);
            game.Logo = Resources.Load<Sprite>(data.logoPath);

            if (game.Icon == null)
                Debug.LogWarning($"Icon not found: {data.iconPath}");

            if (game.Logo == null)
                Debug.LogWarning($"Logo not found: {data.logoPath}");

            game.websiteURL = data.websiteURL;

            game.primaryGenre = data.primaryGenre;
            game.tags = data.tags;

            game.ageRating = data.ageRating;
            game.price = data.price;

            game.releaseYear = data.releaseYear;

            game.criticScore = data.criticScore;
            game.userRatings = data.userRatings;

            game.reviewCount = data.reviewCount;
            game.developer = data.developer;

            // Calculate community average
            game.GenerateUserRating();

            Debug.Log(game.title + " Age Rating: " + game.ageRating);

            // Add to database
            database.games.Add(game);
        }
    }
}
