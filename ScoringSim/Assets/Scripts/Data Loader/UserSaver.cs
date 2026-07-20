using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserSaver : MonoBehaviour
{
    [SerializeField] private UserData user;

    private string SavePath =>
        Path.Combine(Application.persistentDataPath, "UserData.json");

    private void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            DeleteSave();
        }
    }

    public void Save()
    {
        UserJson data = new UserJson();

        data.username = user.username;
        data.likedGenres = user.likedGenres;
        data.likedTags = user.likedTags;
        data.maxAgeRating = user.maxAgeRating;
        data.preferredMaxPrice = user.preferredMaxPrice;
        data.favouriteDevelopers = user.favouriteDevelopers;

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(SavePath, json);

        Debug.Log("User Saved");
    }

    public void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("User save deleted.");
        }
        else
        {
            Debug.Log("No save file found.");
        }
    }
}
