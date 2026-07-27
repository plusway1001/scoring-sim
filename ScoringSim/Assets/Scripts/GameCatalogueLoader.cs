// =====TIFFANY SECTION=====
// This loader connects the CSV game data to the rest of the GameScope systems.
// It reads GamesData.csv, converts each row into a GameEntry object,
// then returns a full list of games for the catalogue, filtering, scoring, and detail pages.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace GameScope
{
    public static class GameCatalogueLoader
    {
        // Reference for the expected CSV columns in Resources/Games/GamesData.csv.
        // The actual loading still reads from the CSV header row so it can match data by column name.
        private static readonly string[] Columns =
        {
            "id","title","genre","genres","tags","rating","price","year",
            "criticScore","communityAvg","totalRatings","dev","cover","colorHex","storeUrl"
        };

        public static List<GameEntry> LoadAll()
        {
            // This list stores all loaded games before returning them to the main database.
            var result = new List<GameEntry>();

            // Loads GamesData.csv from Assets/Resources/Games/GamesData.csv.
            // Unity Resources.Load is used so the file can be accessed at runtime.
            TextAsset csv = Resources.Load<TextAsset>("Games/GamesData");

            // Safety check: prevents the project from crashing if the CSV file is missing.
            if (csv == null)
            {
                Debug.LogWarning("GamesData.csv not found at Resources/Games/GamesData.csv");
                return result;
            }

            // StringReader allows the CSV text file to be read line by line.
            using (StringReader reader = new StringReader(csv.text))
            {
                // First line is the header row, such as id, title, genre, price, score, etc.
                string headerLine = reader.ReadLine();
                if (headerLine == null) return result;

                string[] headers = SplitCsvLine(headerLine);

                string line;

                // Reads every game row after the header.
                while ((line = reader.ReadLine()) != null)
                {
                    // Skips empty rows so blank CSV lines do not create broken GameEntry objects.
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] fields = SplitCsvLine(line);

                    // Stores each row as a dictionary.
                    // This lets the loader access data by column name, for example row["title"].
                    var row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    for (int i = 0; i < headers.Length && i < fields.Length; i++)
                        row[headers[i].Trim()] = fields[i];

                    // Converts the row dictionary into a usable GameEntry object.
                    result.Add(RowToGameEntry(row));
                }
            }

            // Sorts games by ID so the catalogue order stays consistent.
            result.Sort((a, b) => a.Id.CompareTo(b.Id));

            // Applies extra genre/tag classification data after the main game data is loaded.
            GenreTagCsvLoader.ApplyTo(result);

            return result;
        }

        private static GameEntry RowToGameEntry(Dictionary<string, string> row)
        {
            // Converts CSV string data into the correct data types needed by GameEntry.
            int id = ParseInt(Get(row, "id"));
            string title = Get(row, "title");
            string genre = Get(row, "genre");
            string[] genres = SplitList(Get(row, "genres"));
            string[] tags = SplitList(Get(row, "tags"));
            string rating = Get(row, "rating");
            float price = ParseFloat(Get(row, "price"));
            int year = ParseInt(Get(row, "year"));
            int criticScore = ParseInt(Get(row, "criticScore"));
            float communityAvg = ParseFloat(Get(row, "communityAvg"));
            int totalRatings = ParseInt(Get(row, "totalRatings"));
            string dev = Get(row, "dev");
            string cover = Get(row, "cover");
            string colorHex = Get(row, "colorHex");
            string storeUrl = Get(row, "storeUrl");

            // Creates and returns one GameEntry object using the parsed CSV data.
            return new GameEntry(id, title, genre, genres, tags, rating, price, year,
                criticScore, communityAvg, totalRatings, dev, cover, colorHex, storeUrl);
        }

        // Safely gets a value from the row dictionary.
        // If the column is missing, it returns an empty string instead of causing an error.
        private static string Get(Dictionary<string, string> row, string key) =>
            row.TryGetValue(key, out var v) ? v : "";

        // Genres and tags are stored with semicolons in one CSV cell.
        // Example: "Platformer;Indie;Action"
        // This avoids using commas, because commas would be treated as new CSV columns.
        private static string[] SplitList(string cell) =>
            string.IsNullOrWhiteSpace(cell)
                ? new string[0]
                : cell.Split(';');

        // Safely converts text into an integer.
        // If conversion fails, it returns 0 instead of crashing.
        private static int ParseInt(string s) =>
            int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0;

        // Safely converts text into a float.
        // CultureInfo.InvariantCulture keeps decimal parsing consistent.
        private static float ParseFloat(string s) =>
            float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0f;

        // Splits one CSV line into fields.
        // This project uses a simple CSV format, so Split(',') is enough.
        // TrimEnd removes Windows line ending characters.
        private static string[] SplitCsvLine(string line) => line.TrimEnd('\r').Split(',');
    }
}

// =====END OF TIFFANY SECTION=====