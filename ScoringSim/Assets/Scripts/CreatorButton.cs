using UnityEngine;
using UnityEngine.UI;

public class CreatorButton : MonoBehaviour
{
    public string creatorName;

    private Button button;
    private bool selected = false;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleCreator);
    }

    void ToggleCreator()
    {
        selected = !selected;

        if (selected)
        {
            UserProfileManager.Instance.currentUser.preferences.favouriteCreators.Add(creatorName);
            Debug.Log(creatorName + " selected");
        }
        else
        {
            UserProfileManager.Instance.currentUser.preferences.favouriteCreators.Remove(creatorName);
            Debug.Log(creatorName + " removed");
        }
    }
}