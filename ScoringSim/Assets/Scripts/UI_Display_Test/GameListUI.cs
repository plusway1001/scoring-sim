using UnityEngine;

public class GameListUI : MonoBehaviour
{
    [SerializeField] private Transform content;

    [SerializeField] private GameObject gameButtonPrefab;

    public GameDatabase gamedatabase;

    public void GenerateButtons()
    {
        foreach (GameData game in gamedatabase.games)
        {
            GameObject gamePanel = Instantiate(gameButtonPrefab, content);

            gamedatabase.gamesPanelObj.Add(gamePanel);

            if(gamePanel.GetComponent<GameButtonData>() != null)
            gamePanel.GetComponent<GameButtonData>().Setup(game);
        }
    }
}