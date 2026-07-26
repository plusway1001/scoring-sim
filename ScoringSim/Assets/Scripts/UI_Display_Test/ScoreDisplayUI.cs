// === CODE FROM main2/Assets/Scripts/UI_Display_Test/ScoreDisplayUI.cs === (partial, as found already dropped into this project) ===
// Verbatim from the team's main2 branch — no logic changes. This is main2's own
// UGUI/TextMeshPro test-harness script for eyeballing score calculations against a
// GameDatabase; it's not wired into the live VideoScope/GameScope UI Toolkit app
// (that app's actual score display is Scripts/Pages/GameDetailPageController.cs).
// Kept here as-is since it was already present in the project before this merge —
// see Scripts/Scoring/ScoringBridge.cs's header for why this needed the GameData
// type to gain title/Icon/Logo/websiteURL fields (this file is why).
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayUI : MonoBehaviour
{
    [Header("References")]

    private GameData gamedatatemp;

    public GameDatabase database;

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

    [Header("Loop by Index")]

    private int currentGame = 0;

    public int StartGameID = 0;

    private void Start()
    {
        ShowGameByIndex(StartGameID);
    }

    /*void Start()
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
    }*/

    public void ShowGameByClicked(GameData game)
    {
        gamedatatemp = game;

        UpdateUI();
    }

    public void ShowGameByIndex(int index)
    {
        gamedatatemp = database.games[index];

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (gamedatatemp == null || userData == null)
            return;

        ScoreBreakdown breakdown =
            ScoreManager.GetScoreBreakdown(gamedatatemp, userData);

        generalScoreText.text = breakdown.generalScore.ToString("F0");

        userScoreText.text = breakdown.finalScore.ToString("F0");

        genreModifierText.text = breakdown.genreModifier.ToString("+0;-0");

        tagModifierText.text = breakdown.tagModifier.ToString("+0;-0");

        priceModifierText.text = breakdown.priceModifier.ToString("+0;-0");

        developerModifierText.text = breakdown.developerModifier.ToString("+0;-0");

        finalScoreText.text = breakdown.finalScore.ToString("F0");

        Icon.sprite = gamedatatemp.Icon;
        Logo.sprite = gamedatatemp.Logo;
    }

    public void NextGame()
    {
        currentGame++;

        if (currentGame >= database.games.Count)
            currentGame = 0;

        ShowGameByIndex(currentGame);
    }

    public void PreviousGame()
    {
        currentGame--;

        if (currentGame < 0)
            currentGame = database.games.Count - 1;

        ShowGameByIndex(currentGame);
    }

    public void OnGameClicked(GameData selectedGame)
    {
        gamedatatemp = selectedGame;

        ShowGameByClicked(selectedGame);
    }

    public void OpenWebsite()
    {
        if (gamedatatemp == null)
            return;

        if (string.IsNullOrEmpty(gamedatatemp.websiteURL))
            return;

        Application.OpenURL(gamedatatemp.websiteURL);
    }
}