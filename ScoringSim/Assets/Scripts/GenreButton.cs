using UnityEngine;
using UnityEngine.UI;

public class GenreButton : MonoBehaviour
{
    public string genreName;

    private Button button;
    private Image image;

    private bool isSelected = false;

    // Change these to any colours you like
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(1f, 0.45f, 0.6f);

    private void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        button.onClick.AddListener(ToggleGenre);

        image.color = normalColor;
    }

    private void ToggleGenre()
    {
        if (!isSelected)
        {
            UserProfileManager.Instance.currentUser.preferences.likedGenres.Add(genreName);

            image.color = selectedColor;

            isSelected = true;

            Debug.Log(genreName + " selected");
        }
        else
        {
            UserProfileManager.Instance.currentUser.preferences.likedGenres.Remove(genreName);

            image.color = normalColor;

            isSelected = false;

            Debug.Log(genreName + " removed");
        }
    }
}
