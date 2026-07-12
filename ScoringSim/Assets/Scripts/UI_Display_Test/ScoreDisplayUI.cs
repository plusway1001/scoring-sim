using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ScoreDisplayUI : MonoBehaviour
{
    [Header("References")]

    private GameData gamedatatemp;

    public List<GameData> gamesDatabase = new();

    public GameDatabase database;

    public UserData userData;

    public GameDatabase databasePanel;

    [Header("Game Title/ID")]

    public TMP_Text gameTitle;
    public TMP_Text gameID;

    [Header("Score Text")]

    public TMP_Text generalScoreText;

    public TMP_Text userScoreText;

    [Header("Icon/Logo")]

    public Image Icon;

    public Image Logo;

    [Header("General Score Breakdown")]

    public TMP_Text CriticScoreText;

    public TMP_Text CommunityAvgText;

    public TMP_Text VolBonusText;

    public TMP_Text NostalgaFactorText;

    public TMP_Text GeneralScoreFinalText;

    [Header("User Score Breakdown")]

    public TMP_Text genreModifierText;

    public TMP_Text tagModifierText;

    public TMP_Text AgeRatingText;

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

    public string GetGeneralScores(GameData data)
    {
        if (data == null || userData == null)
            return "Invalid";

        data.GenerateUserRating();

        ScoreBreakdown breakdown =
            ScoreManager.GetScoreBreakdown(data, userData);

        return breakdown.generalScore.ToString("F0");
    }

    public string GetUserScores(GameData data)
    {
        if (data == null || userData == null)
            return "Invalid";

        data.GenerateUserRating();

        ScoreBreakdown breakdown =
            ScoreManager.GetScoreBreakdown(data, userData);

        return breakdown.finalScore.ToString("F0");
    }

    public void UpdateUI()
    {
        if (gamedatatemp == null || userData == null)
            return;

        gamedatatemp.GenerateUserRating();

        ScoreBreakdown breakdown =
            ScoreManager.GetScoreBreakdown(gamedatatemp, userData);

        generalScoreText.text = breakdown.generalScore.ToString("F0");

        GeneralScoreFinalText.text = breakdown.generalScore.ToString("F0");

        userScoreText.text = breakdown.finalScore.ToString("F0");

        genreModifierText.text = breakdown.genreModifier.ToString("+0;-0");

        tagModifierText.text = breakdown.tagModifier.ToString("+0;-0");

        if (breakdown.ageRestricted)
        {
            AgeRatingText.text = "NotOK!";
        }
        else
        {
            AgeRatingText.text = "OK!";
        }

        priceModifierText.text = breakdown.priceModifier.ToString("+0;-0");

        developerModifierText.text = breakdown.developerModifier.ToString("+0;-0");

        finalScoreText.text = breakdown.finalScore.ToString("F0");

        Icon.sprite = gamedatatemp.Icon;
        Logo.sprite = gamedatatemp.Logo;
        gameTitle.text = gamedatatemp.title.ToString();
        gameID.text = gamedatatemp.gameID.ToString();

        //GeneralBreakdownScore();
        float volumeBonus = Mathf.Clamp01(Mathf.Log10(gamedatatemp.reviewCount + 1) / 5f) * 100f;
        int age = System.DateTime.Now.Year - gamedatatemp.releaseYear;
        float nostalgia = Mathf.Clamp(age * 2f, 0, 100);
        float CommunityAvgPercentile = (gamedatatemp.communityAverage / 10) * 100;
        gamedatatemp.GenerateUserRating();

        CriticScoreText.text = gamedatatemp.criticScore.ToString("F0");
        CommunityAvgText.text = CommunityAvgPercentile.ToString("F0");
        VolBonusText.text = volumeBonus.ToString("F0");
        NostalgaFactorText.text = nostalgia.ToString("F0");
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

    public void AddToWishlist()
    {
        if (gamedatatemp == null)
            return;
        if (!userData.wishlist.Contains(gamedatatemp))
            userData.wishlist.Add(gamedatatemp);
    }

    public void RemoveFromWishlist()
    {
        if (gamedatatemp == null)
            return;
        if (userData.wishlist.Contains(gamedatatemp))
        {
            userData.wishlist.Remove(gamedatatemp);
        }
    }

    /*public void AddToCompleted()
    {
        if (gamedatatemp == null)
            return;
        if (!userData.completedGames.Contains(gamedatatemp))
            userData.completedGames.Add(gamedatatemp);
    }*/

    /*public void GeneralBreakdownScore()
    {
        float volumeBonus = Mathf.Clamp01(Mathf.Log10(gamedatatemp.reviewCount + 1) / 5f) * 100f;
        int age = System.DateTime.Now.Year - gamedatatemp.releaseYear;
        float nostalgia = Mathf.Clamp(age * 2f, 0, 100);
        gamedatatemp.GenerateUserRating();

        CriticScoreText.text = gamedatatemp.criticScore.ToString("F0");
        CommunityAvgText.text = gamedatatemp.communityAverage.ToString("F0");
        VolBonusText.text = volumeBonus.ToString("F0");
        NostalgaFactorText.text = nostalgia.ToString("F0");
    }*/

    public void SortWishlistGames()
    {
        if (databasePanel.gamesPanelObj == null || userData.wishlist == null)
            return;

        HideAllGamesPanelObj();

        for (int i = 0; i < databasePanel.gamesPanelObj.Count; i++)
        {
            for(int j = 0; j < userData.wishlist.Count; j++)
            {
                if (databasePanel.gamesPanelObj[i].GetComponent<GameButtonData>() == null) return;
                if (databasePanel.gamesPanelObj[i].GetComponent<GameButtonData>().game.gameID 
                    == userData.wishlist[j].gameID)
                {
                    databasePanel.gamesPanelObj[i].SetActive(true);
                }
            }
        }
    }

    public void HideAllGamesPanelObj()
    {
        for (int i = 0; i < databasePanel.gamesPanelObj.Count; i++)
        {
             databasePanel.gamesPanelObj[i].SetActive(false);
        }
    }

    public void ShowAllGamesPanelObj()
    {
        for (int i = 0; i < databasePanel.gamesPanelObj.Count; i++)
        {
            databasePanel.gamesPanelObj[i].SetActive(true);
        }
    }

    public void RateGame(int rating)
    {
        if (gamedatatemp == null)
            return;

        if (rating >= 8)
        {
            if (!userData.favouriteDevelopers.Contains(gamedatatemp.developer))
                userData.favouriteDevelopers.Add(gamedatatemp.developer);
        }
        else
        {
            if (userData.favouriteDevelopers.Contains(gamedatatemp.developer))
                userData.favouriteDevelopers.Remove(gamedatatemp.developer);
        }

        //gamedatatemp.UpdateNewUserRating(rating);

        for (int j = 0; j < gamesDatabase.Count; j++)
        {
            bool isSimilar = false;

            // Genre match
            if (gamesDatabase[j].primaryGenre == gamedatatemp.primaryGenre)
            {
                isSimilar = true;
            }
            else
            {
                // Tag match
                foreach (string tag in gamedatatemp.tags)
                {
                    if (gamesDatabase[j].tags.Contains(tag))
                    {
                        isSimilar = true;
                        break;
                    }
                }
            }

            if (isSimilar)
            {
                gamesDatabase[j].UpdateNewUserRating(rating);
            }
        }

        /*for (int j = 0; j < gamesDatabase.Count; j++)
        {
            if (gamesDatabase[j].primaryGenre.Contains(gamedatatemp.primaryGenre))
            {
                gamesDatabase[j].UpdateNewUserRating(rating);
                continue; // Found at least one matching tag
            }
            foreach (string tag in gamedatatemp.tags)
            {
                if (gamesDatabase[j].tags.Contains(tag))
                {
                    gamesDatabase[j].UpdateNewUserRating(rating);
                    continue;    // Found at least one matching tag
                }
            }
        }*/
    }
}