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

    void Update()
    {
        if (game != null) {
            generalscoreText.text = displayUI.GetGeneralScores(game);
            userscoreText.text = displayUI.GetUserScores(game);
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
    }

    public void OnClickGame()
    {
        displayUI.ShowGameByClicked(game);
    }
}
