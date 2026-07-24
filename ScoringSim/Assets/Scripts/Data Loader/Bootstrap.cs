using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public UserLoader userLoader;
    public GameLoader gameLoader;
    public ScoreDisplayUI scoreUI;
    public GameListUI gameListUI;
    public GameSorter gameSorter;

    private void Awake()
    {
        userLoader.LoadUserUpdated();

        gameLoader.LoadGames();

        scoreUI.Initialize();

        gameListUI.GenerateButtons();

        gameSorter.SortAll();
    }
}
