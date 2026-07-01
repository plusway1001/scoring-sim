using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayUI : MonoBehaviour
{
    [Header("References")]

    public GameData gameData;

    public UserData userData;

    [Header("Score Text")]

    public TMP_Text generalScoreText;

    public TMP_Text userScoreText;

    [Header("Icon/Logo")]

    public Image Icon;
    public Image Logo;

    [Header("Breakdown")]

    public TMP_Text genreModifierText;

    public TMP_Text tagModifierText;

    public TMP_Text priceModifierText;

    public TMP_Text developerModifierText;

    public TMP_Text finalScoreText;

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        ScoreBreakdown breakdown =
            ScoreManager.GetScoreBreakdown(gameData, userData);

        generalScoreText.text =
            breakdown.generalScore.ToString("F0");

        userScoreText.text =
            breakdown.finalScore.ToString("F0");

        genreModifierText.text =
            breakdown.genreModifier.ToString("+0;-0");

        tagModifierText.text =
            breakdown.tagModifier.ToString("+0;-0");

        priceModifierText.text =
            breakdown.priceModifier.ToString("+0;-0");

        developerModifierText.text =
            breakdown.developerModifier.ToString("+0;-0");

        finalScoreText.text =
            breakdown.finalScore.ToString("F0");

        Icon.sprite = gameData.Icon;
        Logo.sprite = gameData.Logo;
    }
}