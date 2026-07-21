using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSorter : MonoBehaviour
{
    [SerializeField] private GameDatabase gameDatabase;

    /// <summary>
    /// Sorts the UI so that the selected genre appears first.
    /// </summary>
    public void SortByGenre(string genre)
    {
        List<GameObject> sorted = gameDatabase.gamesPanelObj
            .OrderByDescending(panel =>
            {
                GameButtonData button = panel.GetComponent<GameButtonData>();

                if (button == null || button.game == null)
                    return false;

                return button.game.primaryGenre.Equals(
                    genre,
                    System.StringComparison.OrdinalIgnoreCase);
            })
            .ThenBy(panel =>
            {
                GameButtonData button = panel.GetComponent<GameButtonData>();

                return button.game.title;
            })
            .ToList();

        // Move the UI objects
        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].transform.SetSiblingIndex(i);
        }

        // Keep the database list in the same order
        gameDatabase.gamesPanelObj = sorted;
    }

    // Optional shortcut methods for UI buttons

    public void SortHorror()
    {
        SortByGenre("Horror");
    }

    public void SortRPG()
    {
        SortByGenre("RPG");
    }

    public void SortPlatformer()
    {
        SortByGenre("Platformer");
    }

    public void SortSports()
    {
        SortByGenre("Sports");
    }

    public void SortFighting()
    {
        SortByGenre("Fighting");
    }

    public void SortOpenWorld()
    {
        SortByGenre("Open World");
    }

    public void SortAll()
    {
        List<GameObject> sorted = gameDatabase.gamesPanelObj
            .OrderBy(panel =>
            {
                GameButtonData button = panel.GetComponent<GameButtonData>();
                return button.game.title;
            })
            .ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].transform.SetSiblingIndex(i);
        }

        gameDatabase.gamesPanelObj = sorted;
    }
}