using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GenreTagDataLoader : MonoBehaviour
{
    [Header("Genre + Tag CSV")]
    [SerializeField] private TextAsset genreTagCSV;

    [Header("Loaded Game Data")]
    public List<GameGenreData> games = new List<GameGenreData>();

    void Awake()
    {
        LoadGenreTagCSV();
        PrintLoadedData();
    }

    void LoadGenreTagCSV()
    {
        if (genreTagCSV == null)
        {
            Debug.LogWarning("Genre/Tag CSV is missing.");
            return;
        }

        using (StringReader reader = new StringReader(genreTagCSV.text))
        {
            string line;
            string[] headers = null;

            bool isReadingGenre = false;
            bool isReadingTag = false;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] fields = line.Split(',');
                string firstColumn = fields[0].Trim().Trim('\uFEFF');

                if (firstColumn == "Genre Name")
                {
                    headers = fields;
                    isReadingGenre = true;
                    isReadingTag = false;

                    CreateGamesFromHeaders(headers);
                    continue;
                }

                if (firstColumn == "Game Tag")
                {
                    headers = fields;
                    isReadingGenre = false;
                    isReadingTag = true;

                    CreateGamesFromHeaders(headers);
                    continue;
                }

                if (headers == null)
                    continue;

                string categoryName = firstColumn;

                for (int i = 1; i < fields.Length && i < headers.Length; i++)
                {
                    string gameTitle = headers[i].Trim();
                    bool hasCategory = ParseBool(fields[i]);

                    if (!hasCategory)
                        continue;

                    GameGenreData game = GetGameByTitle(gameTitle);

                    if (game == null)
                        continue;

                    if (isReadingGenre)
                    {
                        if (!game.genres.Contains(categoryName))
                        {
                            game.genres.Add(categoryName);
                        }
                    }
                    else if (isReadingTag)
                    {
                        if (!game.tags.Contains(categoryName))
                        {
                            game.tags.Add(categoryName);
                        }
                    }
                }
            }
        }
    }

    void CreateGamesFromHeaders(string[] headers)
    {
        for (int i = 1; i < headers.Length; i++)
        {
            string gameTitle = headers[i].Trim();

            if (string.IsNullOrWhiteSpace(gameTitle))
                continue;

            if (GetGameByTitle(gameTitle) == null)
            {
                GameGenreData newGame = new GameGenreData();
                newGame.title = gameTitle;

                games.Add(newGame);
            }
        }
    }

    bool ParseBool(string value)
    {
        string cleanedValue = value.Trim().ToLower();

        return cleanedValue == "true"
            || cleanedValue == "y"
            || cleanedValue == "yes"
            || cleanedValue == "1";
    }

    // Finds a game by title
    public GameGenreData GetGameByTitle(string title)
    {
        return games.Find(g => g.title == title);
    }

    // Returns all genres for a selected game
    public List<string> GetGenresForGame(string title)
    {
        GameGenreData game = GetGameByTitle(title);

        if (game == null)
            return new List<string>();

        return game.genres;
    }
    
    // Returns all tags for a selected game
    public List<string> GetTagsForGame(string title)
    {
        GameGenreData game = GetGameByTitle(title);

        if (game == null)
            return new List<string>();

        return game.tags;
    }
    // Checks if a selected game has a specific genre
    public bool HasGenre(string title, string genreName)
    {
        GameGenreData game = GetGameByTitle(title);

        if (game == null)
            return false;

        return game.genres.Contains(genreName);
    }
    // Checks if a selected game has a specific tag
    public bool HasTag(string title, string tagName)
    {
        GameGenreData game = GetGameByTitle(title);

        if (game == null)
            return false;

        return game.tags.Contains(tagName);
    }

    void PrintLoadedData()
    {
        foreach (GameGenreData game in games)
        {
            Debug.Log(
                game.title +
                "\nGenres: " + string.Join(", ", game.genres) +
                "\nTags: " + string.Join(", ", game.tags)
            );
        }
    }
}