using UnityEngine;
using UnityEngine.UI;

public class CreatorButton : MonoBehaviour
{
    public string creatorName;
    private Image buttonImage;
    private Button button;
    private bool selected = false;

    public Color normalColor = Color.white;
    public Color selectedColor = new Color(1f, 0.45f, 0.6f);

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        button.onClick.AddListener(ToggleCreator);

        buttonImage.color = normalColor;
    }

    void ToggleCreator()
    {
        selected = !selected;

        if (selected)
        {
            UserProfileManager.Instance.currentUser.preferences.favouriteCreators.Add(creatorName);
            buttonImage.color = selectedColor;
            Debug.Log(creatorName + " selected");
        }
        else
        {
            UserProfileManager.Instance.currentUser.preferences.favouriteCreators.Remove(creatorName);
            buttonImage.color = normalColor;
            Debug.Log(creatorName + " removed");
        }
    }
}