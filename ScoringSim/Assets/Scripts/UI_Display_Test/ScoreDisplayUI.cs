using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public enum PageState
{
    Home,
    Wishlist
}

public class ScoreDisplayUI : MonoBehaviour
{
    [Header("References")]

    private GameData gamedatatemp;

    public List<GameData> gamesDatabase = new();

    public GameDatabase database;

    public UserData userData;

    public GameDatabase databasePanel;

    private PageState pagestatus;

    [Header("Game Info/Details")]

    public TMP_Text gameTitle;
    public TMP_Text gameID;
    public TMP_Text yearText;
    public TMP_Text genreText;
    public TMP_Text priceText;
    public TMP_Text tagsText;

    [Header("Dashboard Info")]

    public TMP_Text wishlistNum;

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

    [Header("Wishlist UI")]

    public GameObject AddWishlistBtn, RemoveWishlistBtn, OpenWebsiteBtn;

    [Header("Game Rating UI")]

    public GameObject CompletedGamesBtn, GameRatingBtn, GameRatedDisplay;
    public List<GameObject> ratingsObj = new();
    private int tempRating;

    private void Start()
    {
        ShowGameByIndex(StartGameID);
        EnterHomePage();
    }

    private void Update()
    {
        wishlistNum.text = userData.wishlist.Count.ToString();
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

        yearText.text = gamedatatemp.releaseYear.ToString("F0");
        genreText.text = gamedatatemp.primaryGenre;
        priceText.text = "S$" + gamedatatemp.price.ToString("F0");

        tagsText.text = string.Join(", ", gamedatatemp.tags);

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

    public void CompletedGames()
    {
        if (gamedatatemp == null)
            return;
        if (!userData.completedGames.Contains(gamedatatemp))
        {
            userData.completedGames.Add(gamedatatemp);
        }
        CheckGameRatingStatus();
    }

    public void CheckGameRatingStatus()
    {
        if (gamedatatemp == null)
            return;
        if (!userData.completedGames.Contains(gamedatatemp))
        {
            CompletedGamesBtn.SetActive(true);
            GameRatingBtn.SetActive(false);
            GameRatedDisplay.SetActive(false);
        }
        else
        {
            if (!userData.gamesrating.Contains(gamedatatemp))
            {
                CompletedGamesBtn.SetActive(false);
                GameRatingBtn.SetActive(true);
                GameRatedDisplay.SetActive(false);
            }
            else
            {
                CompletedGamesBtn.SetActive(false);
                GameRatingBtn.SetActive(false);
                GameRatedDisplay.SetActive(true);
            }
        }
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
        {
            userData.wishlist.Add(gamedatatemp);
        }
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

    public void CheckWishlistBtnStatus()
    {
        if (gamedatatemp == null)
            return;
        if (!userData.wishlist.Contains(gamedatatemp))
        {
            AddWishlistBtn.SetActive(true);
            RemoveWishlistBtn.SetActive(false);
            OpenWebsiteBtn.SetActive(false);
        }
        else
        {
            AddWishlistBtn.SetActive(false);
            RemoveWishlistBtn.SetActive(true);
            OpenWebsiteBtn.SetActive(true);
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

    public void CheckForWishlistPage()
    {
        if(pagestatus == PageState.Wishlist)
        {
            SortWishlistGames();
        }
    }

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

    public void EnterHomePage()
    {
        pagestatus = PageState.Home;
    }
    public void EnterWishlistPage()
    {
        pagestatus = PageState.Wishlist;
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

        hideAllGamesRatingStars();
        showNumberOfGamesRatingStars(rating);
        if (!userData.gamesrating.Contains(gamedatatemp))
        {
            userData.gamesrating.Add(gamedatatemp);
        }
        CheckGameRatingStatus();

        gamedatatemp.yourRatings = rating;

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

    public void hideAllGamesRatingStars()
    {
        foreach (GameObject obj in ratingsObj)
        {
            obj.SetActive(false);
        }
    }

    public void showNumberOfGamesRatingStars(int rating)
    {
        for(int a = 0; a < rating; a++)
        {
            ratingsObj[a].SetActive(true);
        }
    }

    public void CheckGameRatingStarsStatus()
    {
        if (gamedatatemp == null)
            return;
        hideAllGamesRatingStars();
        showNumberOfGamesRatingStars(gamedatatemp.yourRatings);
    }
}