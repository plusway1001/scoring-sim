using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Loads game tag data from a CSV file and stores which tags belong to each game.
public class GameTagDataLoader : MonoBehaviour
{
    [SerializeField] private TextAsset tagCSV;

    public List<GameTagData> gameTags = new List<GameTagData>();

    void Awake()
    {
        LoadTagCSV();
        PrintLoadedTags();
    }

    void LoadTagCSV()
    {
        if (tagCSV == null)
        {
            Debug.LogWarning("Tag CSV is missing.");
            return;
        }

        using (StringReader reader = new StringReader(tagCSV.text))
        {
            string headerLine = reader.ReadLine();

            if (string.IsNullOrWhiteSpace(headerLine))
            {
                Debug.LogWarning("Tag CSV is empty.");
                return;
            }

            string[] headers = headerLine.Split(',');

            for (int i = 1; i < headers.Length; i++)
            {
                string gameName = headers[i].Trim();

                if (string.IsNullOrWhiteSpace(gameName))
                    continue;

                GameTagData data = new GameTagData();
                data.gameName = gameName;

                gameTags.Add(data);
            }

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] fields = line.Split(',');

                if (fields.Length < 2)
                    continue;

                string tagName = fields[0].Trim();

                for (int i = 1; i < fields.Length && i < headers.Length; i++)
                {
                    bool hasTag = ParseBool(fields[i]);

                    if (hasTag)
                    {
                        if (!gameTags[i - 1].tags.Contains(tagName))
                        {
                            gameTags[i - 1].tags.Add(tagName);
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

    // Returns all tags assigned to a selected game.
    public List<string> GetTagsForGame(string gameName)
    {
        GameTagData data = gameTags.Find(g => g.gameName == gameName);

        if (data == null)
        {
            return new List<string>();
        }

        return data.tags;
    }

    // Checks if a selected game has a specific tag.
    public bool HasTag(string gameName, string tagName)
    {
        GameTagData data = gameTags.Find(g => g.gameName == gameName);

        if (data == null)
        {
            return false;
        }

        return data.tags.Contains(tagName);
    }

    void PrintLoadedTags()
    {
        foreach (GameTagData data in gameTags)
        {
            Debug.Log(data.gameName + " Tags: " + string.Join(", ", data.tags));
        }
    }
}

// Stores the tag list for one game.
//E.g for a game called "Game A" with tags "Action" and "Multiplayer", the GameTagData would look like:
[System.Serializable]
public class GameTagData
{
    public string gameName;
    public List<string> tags = new List<string>();
}