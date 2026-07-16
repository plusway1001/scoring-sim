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

    public TMP_Text LikeGenreNum;

    public TMP_Text GameCompletedNum;

    public TMP_Text GameRatedNum;

    public TMP_Text GameMatchesNum;

    public TMP_Text UserName;

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

    public Slider CriticScore;
    public Slider CommunityAvg;
    public Slider VolBonus;
    public Slider NostalgaFactor;

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

    [Header("Game Great Matches")]

    public float matchingscoreLimit;

    private void Start()
    {
        ShowGameByIndex(StartGameID);
        EnterHomePage();
    }

    private void Update()
    {
        DashboardStatus();
    }

    public void DashboardStatus()
    {
        wishlistNum.text = userData.wishlist.Count.ToString();
        GameMatchesNum.text = userData.greatmatches.Count.ToString();
        LikeGenreNum.text = userData.likedGenres.Count.ToString();
        GameCompletedNum.text = userData.completedGames.Count.ToString();
        GameRatedNum.text = userData.gamesrating.Count.ToString();
        UserName.text = "Welcome back, " + userData.username;
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

    public void DiffScores(GameData data)
    {
        if (data == null || userData == null)
            return;

        float diffscores;

        data.GenerateUserRating();

        ScoreBreakdown breakdown =
            ScoreManager.GetScoreBreakdown(data, userData);

        if (breakdown.generalScore > breakdown.finalScore)
        {
            diffscores = breakdown.generalScore - breakdown.finalScore;
        }
        else
        {
            diffscores = breakdown.finalScore - breakdown.generalScore;
        }

        if(diffscores < matchingscoreLimit)
        {
            if(!userData.greatmatches.Contains(data))
            userData.greatmatches.Add(data);
        }
        else
        {
            if (userData.greatmatches.Contains(data))
                userData.greatmatches.Remove(data);
        }
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

    public void ChangeValueColour(ScoreBreakdown breakdown)
    {
        if (breakdown.genreModifier > 0)
        {
            genreModifierText.color = new Color32(144, 238, 144, 255); // Light Green
        }
        else if (breakdown.genreModifier < 0)
        {
            genreModifierText.color = new Color32(255, 182, 193, 255); // Light Red
        }
        else
        {
            genreModifierText.color = Color.white; // Neutral
        }

        if (breakdown.tagModifier > 0)
        {
            tagModifierText.color = new Color32(144, 238, 144, 255); // Light Green
        }
        else if (breakdown.tagModifier < 0)
        {
            tagModifierText.color = new Color32(255, 182, 193, 255); // Light Red
        }
        else
        {
            tagModifierText.color = Color.white; // Neutral
        }

        if (breakdown.priceModifier > 0)
        {
            priceModifierText.color = new Color32(144, 238, 144, 255); // Light Green
        }
        else if (breakdown.priceModifier < 0)
        {
            priceModifierText.color = new Color32(255, 182, 193, 255); // Light Red
        }
        else
        {
            priceModifierText.color = Color.white; // Neutral
        }

        if (breakdown.developerModifier > 0)
        {
            developerModifierText.color = new Color32(144, 238, 144, 255); // Light Green
        }
        else if (breakdown.developerModifier < 0)
        {
            developerModifierText.color = new Color32(255, 182, 193, 255); // Light Red
        }
        else
        {
            developerModifierText.color = Color.white; // Neutral
        }
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
            AgeRatingText.color = new Color32(255, 182, 193, 255); // Light Red
        }
        else
        {
            AgeRatingText.text = "OK!";
            AgeRatingText.color = new Color32(144, 238, 144, 255); // Light Green
        }

        priceModifierText.text = breakdown.priceModifier.ToString("+0;-0");

        developerModifierText.text = breakdown.developerModifier.ToString("+0;-0");

        finalScoreText.text = breakdown.finalScore.ToString("F0");

        ChangeValueColour(breakdown);

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

        CriticScore.maxValue = 40f;
        CommunityAvg.maxValue = 35f;
        VolBonus.maxValue = 15f;
        NostalgaFactor.maxValue = 10f;

        CriticScore.minValue = 0f;
        CommunityAvg.minValue = 0f;
        VolBonus.minValue = 0f;
        NostalgaFactor.minValue = 0f;

        CriticScore.value = gamedatatemp.criticScore * 0.40f;

        //Debug.Log("critics"+ CriticScore.value);

        CommunityAvg.value = CommunityAvgPercentile * 0.35f;
        VolBonus.value = volumeBonus * 0.15f;
        NostalgaFactor.value = nostalgia * 0.10f;
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