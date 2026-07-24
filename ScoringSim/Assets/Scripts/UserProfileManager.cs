using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class UserProfileManager : MonoBehaviour
{
    // Singleton so other scripts can access this manager
    public static UserProfileManager Instance;

    // Stores the current user's data
    public User currentUser = new User();

    public int sceneIndex = 1;
    public string userName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Temporary username
        currentUser.username = userName;

        
    }

    // =========================
    // Genre Management
    // =========================

    public void AddLikedGenre(string genre)
    {
        if (!currentUser.preferences.likedGenres.Contains(genre))
        {
            currentUser.preferences.likedGenres.Add(genre);

            Debug.Log($"Genre Added: {genre}");
        }
    }

    public void RemoveLikedGenre(string genre)
    {
        if (currentUser.preferences.likedGenres.Contains(genre))
        {
            currentUser.preferences.likedGenres.Remove(genre);

            Debug.Log($"Genre Removed: {genre}");
        }
    }

    // =========================
    // Save User
    // =========================

    public void SaveUser()
    {
        // Path to Assets/JSON/user.json
        string path = Path.Combine(Application.dataPath, "JSON/user.json");

        UserDatabase database = new UserDatabase();

        // Load existing users if the file exists
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            if (!string.IsNullOrWhiteSpace(json))
            {
                UserDatabase loadedDatabase = JsonUtility.FromJson<UserDatabase>(json);

                if (loadedDatabase != null)
                {
                    database = loadedDatabase;
                }
            }
        }

        // Add the current user
        database.users.Add(currentUser);

        // Convert to formatted JSON
        string output = JsonUtility.ToJson(database, true);

        // Save to user.json
        File.WriteAllText(path, output);

        Debug.Log("User saved successfully!");
        Debug.Log(output);

        SceneManager.LoadScene(sceneIndex);
    }

    
}