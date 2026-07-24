using System.IO;
using UnityEngine;

public class UserLoader : MonoBehaviour
{
    [SerializeField] private UserData user;

    public void LoadUser()
    {
        TextAsset json =
            Resources.Load<TextAsset>("User/UserData");

        if (json == null)
        {
            Debug.Log("No user found.");
            return;
        }

        UserJson data =
            JsonUtility.FromJson<UserJson>(json.text);

        user.username = data.username;

        user.likedGenres = data.likedGenres;

        user.likedTags = data.likedTags;

        user.maxAgeRating = data.maxAgeRating;

        user.preferredMaxPrice = data.preferredMaxPrice;

        user.favouriteDevelopers =
            data.favouriteDevelopers;

        Debug.Log("=== USER LOADED ===");
        Debug.Log("Username: " + user.username);
        Debug.Log("Max Age: " + user.maxAgeRating);
    }

    public void LoadUserUpdated()
    {
        // Assets/JSON/user.json
        string path = Path.Combine(Application.dataPath, "JSON/user.json");

        if (!File.Exists(path))
        {
            Debug.LogWarning("User file not found: " + path);
            return;
        }

        string json = File.ReadAllText(path);

        UserDatabase database = JsonUtility.FromJson<UserDatabase>(json);

        if (database == null || database.users == null || database.users.Count == 0)
        {
            Debug.LogWarning("No users found in JSON.");
            return;
        }

        // Load the first user
        User loadedUser = database.users[0];

        user.username = loadedUser.username;
        user.likedGenres = new System.Collections.Generic.List<string>(loadedUser.preferences.likedGenres);
        user.likedTags = new System.Collections.Generic.List<string>(loadedUser.preferences.likedTags);
        user.maxAgeRating = loadedUser.preferences.maxAgeRating;
        user.preferredMaxPrice = loadedUser.preferences.maxPrice;
        user.favouriteDevelopers = new System.Collections.Generic.List<string>(loadedUser.preferences.favouriteCreators);

        Debug.Log("=== USER LOADED ===");
        Debug.Log("Username: " + user.username);
        Debug.Log("Genres: " + string.Join(", ", user.likedGenres));
        Debug.Log("Tags: " + string.Join(", ", user.likedTags));
        Debug.Log("Favourite Developers: " + string.Join(", ", user.favouriteDevelopers));
        Debug.Log("Max Age: " + user.maxAgeRating);
        Debug.Log("Max Price: $" + user.preferredMaxPrice);
    }
}
