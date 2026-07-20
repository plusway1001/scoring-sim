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
}
