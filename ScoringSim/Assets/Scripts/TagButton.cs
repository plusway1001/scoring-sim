using UnityEngine;
using UnityEngine.UI;

public class TagButton : MonoBehaviour
{
    public string tagName;

    // Colours
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(1f, 0.45f, 0.6f);

    private Button button;
    private Image buttonImage;
    private bool selected = false;

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        button.onClick.AddListener(ToggleTag);

        // Set initial colour
        buttonImage.color = normalColor;
    }

    void ToggleTag()
    {
        selected = !selected;

        if (selected)
        {
            UserProfileManager.Instance.currentUser.preferences.likedTags.Add(tagName);
            buttonImage.color = selectedColor;

            Debug.Log(tagName + " selected");
        }
        else
        {
            UserProfileManager.Instance.currentUser.preferences.likedTags.Remove(tagName);
            buttonImage.color = normalColor;

            Debug.Log(tagName + " removed");
        }
    }
}