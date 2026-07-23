using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TagButton : MonoBehaviour
{
    private Button button;
    private Image buttonImage;
    private TMP_Text text;

    private bool selected = false;

    public Color normalColor = Color.white;
    public Color selectedColor = new Color(1f, 0.45f, 0.6f);

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        text = GetComponentInChildren<TMP_Text>();

        button.onClick.AddListener(ToggleTag);

        buttonImage.color = normalColor;
    }

    void ToggleTag()
    {
        string tagName = text.text;

        if (!selected)
        {
            UserProfileManager.Instance.currentUser.preferences.likedTags.Add(tagName);
            buttonImage.color = selectedColor;
            selected = true;

            Debug.Log(tagName + " selected");
        }
        else
        {
            UserProfileManager.Instance.currentUser.preferences.likedTags.Remove(tagName);
            buttonImage.color = normalColor;
            selected = false;

            Debug.Log(tagName + " removed");
        }
    }
}