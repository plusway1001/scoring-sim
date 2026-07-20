using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public UserLoader userLoader;
    public GameLoader gameLoader;
    public ScoreDisplayUI scoreUI;
    public GameListUI gameListUI;

    private void Awake()
    {
        userLoader.LoadUser();

        gameLoader.LoadGames();

        scoreUI.Initialize();

        gameListUI.GenerateButtons();
    }
}
