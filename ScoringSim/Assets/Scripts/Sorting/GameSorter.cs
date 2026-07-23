using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSorter : MonoBehaviour
{
    [SerializeField] private GameDatabase gameDatabase;

    //==================================================
    // Sort by Genre
    //==================================================

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

        ApplySorting(sorted);
    }

    //==================================================
    // Sort by Tag
    //==================================================

    public void SortByTag(string tag)
    {
        List<GameObject> sorted = gameDatabase.gamesPanelObj
            .OrderByDescending(panel =>
            {
                GameButtonData button = panel.GetComponent<GameButtonData>();

                if (button == null || button.game == null)
                    return false;

                return button.game.tags.Any(t =>
                    t.Equals(tag,
                    System.StringComparison.OrdinalIgnoreCase));
            })
            .ThenBy(panel =>
            {
                GameButtonData button = panel.GetComponent<GameButtonData>();
                return button.game.title;
            })
            .ToList();

        ApplySorting(sorted);
    }

    public void SortByMultipleTags(List<string> selectedTags)
    {
        List<GameObject> sorted = gameDatabase.gamesPanelObj
            .OrderByDescending(panel =>
            {
                GameButtonData button = panel.GetComponent<GameButtonData>();

                if (button == null || button.game == null)
                    return -1;

                return button.game.tags.Count(tag =>
                    selectedTags.Contains(tag));
            })
            .ThenBy(panel =>
            {
                return panel.GetComponent<GameButtonData>().game.title;
            })
            .ToList();

        ApplySorting(sorted);
    }

    //==================================================
    // Restore Original (Alphabetical)
    //==================================================

    public void SortAll()
    {
        List<GameObject> sorted = gameDatabase.gamesPanelObj
            .OrderBy(panel =>
            {
                GameButtonData button = panel.GetComponent<GameButtonData>();
                return button.game.title;
            })
            .ToList();

        ApplySorting(sorted);
    }

    //==================================================
    // Apply Sorting
    //==================================================

    private void ApplySorting(List<GameObject> sorted)
    {
        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].transform.SetSiblingIndex(i);
        }

        gameDatabase.gamesPanelObj = sorted;
    }

    //==================================================
    // Genre Buttons
    //==================================================

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

    public void SortOpenWorldGenre()
    {
        SortByGenre("Open World");
    }

    //==================================================
    // Tag Buttons
    //==================================================

    public void SortMultiplayer()
    {
        SortByTag("Multiplayer");
    }

    public void SortSinglePlayer()
    {
        SortByTag("Single Player");
    }

    public void SortAction()
    {
        SortByTag("Action");
    }

    public void SortAdventure()
    {
        SortByTag("Adventure");
    }

    public void SortStoryRich()
    {
        SortByTag("Story Rich");
    }

    public void SortFantasy()
    {
        SortByTag("Fantasy");
    }

    public void SortPixelArt()
    {
        SortByTag("Pixel Art");
    }

    public void SortPuzzle()
    {
        SortByTag("Puzzle");
    }

    public void SortCoop()
    {
        SortByTag("Co-op");
    }

    public void SortOpenWorldTag()
    {
        SortByTag("Open World");
    }
}