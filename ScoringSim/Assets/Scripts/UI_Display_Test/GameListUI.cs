using UnityEngine;

public class GameListUI : MonoBehaviour
{
    [SerializeField] private Transform content;

    [SerializeField] private GameObject gameButtonPrefab;

    public GameDatabase gamedatabase;

    private void Start()
    {
        foreach (GameData game in gamedatabase.games)
        {
            GameObject gamePanel = Instantiate(gameButtonPrefab, content);

            if(gamePanel.GetComponent<GameButtonData>() != null)
            gamePanel.GetComponent<GameButtonData>().Setup(game);
        }
    }
}