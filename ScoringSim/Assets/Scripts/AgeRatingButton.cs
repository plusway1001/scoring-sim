using UnityEngine;
using UnityEngine.UI;

public class AgeRatingButton : MonoBehaviour
{
    public string rating;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetRating);
    }

    void SetRating()
    {
        UserProfileManager.Instance.currentUser.preferences.maxAgeRating = rating;

        Debug.Log("Age Rating: " + rating);
    }
}