using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonData : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text yearText;
    public TMP_Text genreText;
    public TMP_Text priceText;
    public Image Logo;
    public TMP_Text tagsText;
    public TMP_Text generalscoreText;
    public TMP_Text userscoreText;

    public ScoreDisplayUI displayUI;

    public GameData game;
    public UserData user;

    void Update()
    {
        if (game != null) {
            generalscoreText.text = displayUI.GetGeneralScores(game);
            userscoreText.text = displayUI.GetUserScores(game);
            displayUI.DiffScores(game);
            ChangeColourValueText(game);
        }
    }

    public void Setup(GameData data)
    {
        game = data;

        titleText.text = game.name;
        yearText.text = game.releaseYear.ToString("F0");
        genreText.text = game.primaryGenre;
        priceText.text = "S$" + game.price.ToString("F0");
        Logo.sprite = game.Logo;

        tagsText.text = string.Join(", ", game.tags);

        generalscoreText.text = displayUI.GetGeneralScores(game);
        userscoreText.text = displayUI.GetUserScores(game);
        ChangeColourValueText(game);
    }

    public void OnClickGame()
    {
        displayUI.ShowGameByClicked(game);
    }

    public void ChangeColourValueText(GameData game)
    {
        if (game == null || user == null)
            return;

        game.GenerateUserRating();

        ScoreBreakdown breakdown =
            ScoreManager.GetScoreBreakdown(game, user);

        if (breakdown.generalScore >= 80)
        {
            generalscoreText.color = new Color32(144, 238, 144, 255); // Light Green
        }
        else if (breakdown.generalScore >= 60)
        {
            generalscoreText.color = Color.white; // White
        }
        else if (breakdown.generalScore >= 40)
        {
            generalscoreText.color = new Color32(255, 255, 153, 255); // Light Yellow
        }
        else
        {
            generalscoreText.color = new Color32(255, 182, 193, 255); // Light Red
        }

        if (breakdown.finalScore >= 80)
        {
            userscoreText.color = new Color32(144, 238, 144, 255); // Light Green
        }
        else if (breakdown.finalScore >= 60)
        {
            userscoreText.color = Color.white; // White
        }
        else if (breakdown.finalScore >= 40)
        {
            userscoreText.color = new Color32(255, 255, 153, 255); // Light Yellow
        }
        else
        {
            userscoreText.color = new Color32(255, 182, 193, 255); // Light Red
        }
    }
}
