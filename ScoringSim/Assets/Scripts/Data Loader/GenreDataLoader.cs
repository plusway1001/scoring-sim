using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Loads both genre and tag CSV files, then stores the genres/tags belonging to each game.
public class GenreTagDataLoader : MonoBehaviour
{
    [Header("CSV Files")]
    [SerializeField] private TextAsset genreCSV;
    [SerializeField] private TextAsset tagCSV;

    [Header("Loaded Data")]
    public List<VideoGameData> games = new List<VideoGameData>();

    void Awake()
    {
        LoadGenreCSV();
        LoadTagCSV();
        PrintLoadedData();
    }

    void LoadGenreCSV()
    {
        if (genreCSV == null)
        {
            Debug.LogWarning("Genre CSV is missing.");
            return;
        }

        LoadBooleanTableCSV(genreCSV, true);
    }

    void LoadTagCSV()
    {
        if (tagCSV == null)
        {
            Debug.LogWarning("Tag CSV is missing.");
            return;
        }

        LoadBooleanTableCSV(tagCSV, false);
    }

    void LoadBooleanTableCSV(TextAsset csvFile, bool isGenreCSV)
    {
        using (StringReader reader = new StringReader(csvFile.text))
        {
            string headerLine = reader.ReadLine();

            if (string.IsNullOrWhiteSpace(headerLine))
            {
                Debug.LogWarning("CSV file is empty.");
                return;
            }

            string[] headers = headerLine.Split(',');

            for (int i = 1; i < headers.Length; i++)
            {
                string gameTitle = headers[i].Trim();

                if (string.IsNullOrWhiteSpace(gameTitle))
                    continue;

                if (GetGameByTitle(gameTitle) == null)
                {
                    VideoGameData newGame = new VideoGameData();
                    newGame.title = gameTitle;
                    games.Add(newGame);
                }
            }

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] fields = line.Split(',');

                if (fields.Length < 2)
                    continue;

                string categoryName = fields[0].Trim();

                if (string.IsNullOrWhiteSpace(categoryName))
                    continue;

                for (int i = 1; i < fields.Length && i < headers.Length; i++)
                {
                    string gameTitle = headers[i].Trim();
                    bool hasCategory = ParseBool(fields[i]);

                    if (!hasCategory)
                        continue;

                    VideoGameData game = GetGameByTitle(gameTitle);

                    if (game == null)
                        continue;

                    if (isGenreCSV)
                    {
                        if (!game.genres.Contains(categoryName))
                        {
                            game.genres.Add(categoryName);
                        }
                    }
                    else
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
    bool ParseBool(string value)
    {
        string cleanedValue = value.Trim().ToLower();

        return cleanedValue == "true"
            || cleanedValue == "y"
            || cleanedValue == "yes"
            || cleanedValue == "1";
    }

    // Finds and returns a game by its title.
    public VideoGameData GetGameByTitle(string title)
    {
        return games.Find(g => g.title == title);
    }

    // Returns all genres assigned to a selected game.
    public List<string> GetGenresForGame(string title)
    {
        VideoGameData game = GetGameByTitle(title);

        if (game == null)
        {
            return new List<string>();
        }

        return game.genres;
    }

    // Returns all tags assigned to a selected game.
    public List<string> GetTagsForGame(string title)
    {
        VideoGameData game = GetGameByTitle(title);

        if (game == null)
        {
            return new List<string>();
        }

        return game.tags;
    }

    // Checks if a selected game has a specific genre.
    public bool HasGenre(string title, string genreName)
    {
        VideoGameData game = GetGameByTitle(title);

        if (game == null)
        {
            return false;
        }

        return game.genres.Contains(genreName);
    }

    // Checks if a selected game has a specific tag.
    public bool HasTag(string title, string tagName)
    {
        VideoGameData game = GetGameByTitle(title);

        if (game == null)
        {
            return false;
        }

        return game.tags.Contains(tagName);
    }

    void PrintLoadedData()
    {
        foreach (VideoGameData game in games)
        {
            Debug.Log(
                game.title +
                "\nGenres: " + string.Join(", ", game.genres) +
                "\nTags: " + string.Join(", ", game.tags)
            );
        }
    }
}

// Stores genre, tag, and game information for one video game.
[System.Serializable]
public class VideoGameData
{
    public string gameID;
    public string title;

    public List<string> genres = new List<string>();
    public List<string> tags = new List<string>();

    public string ageRating;
    public float price;
    public int releaseYear;
    public string developer;

    public float criticScore;
    public float communityRatingAverage;
    public float generalScore;
}