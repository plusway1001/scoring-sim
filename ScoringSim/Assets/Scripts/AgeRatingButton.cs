using UnityEngine;
using UnityEngine.UI;

public class AgeRatingButton : MonoBehaviour
{
    public string rating;

    private Button button;

    private Image buttonImage;
    private bool selected = false;

    public Color normalColor = Color.white;
    public Color selectedColor = new Color(1f, 0.45f, 0.6f);

    public ResetButtonColor resetbtncolor;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetRating);

        buttonImage = GetComponent<Image>();

        buttonImage.color = normalColor;
    }

    void SetRating()
    {
        UserProfileManager.Instance.currentUser.preferences.maxAgeRating = rating;

        Debug.Log("Age Rating: " + rating);

        this.selected = !selected;

        if (this.selected)
        {
            //resetbtncolor.ResetButtonColour();
            buttonImage.color = selectedColor;
        }
        else
        {
            buttonImage.color = normalColor;
        }
    }
}