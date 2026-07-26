// =====TIFFANY SECTION=====
// NEW
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace GameScope
{
    public static class GameCatalogueLoader
    {
        // Column order in Resources/Games/GamesData.csv.
        private static readonly string[] Columns =
        {
            "id","title","genre","genres","tags","rating","price","year",
            "criticScore","communityAvg","totalRatings","dev","cover","colorHex","storeUrl"
        };

        public static List<GameEntry> LoadAll()
        {
            var result = new List<GameEntry>();

            TextAsset csv = Resources.Load<TextAsset>("Games/GamesData");
            if (csv == null)
            {
                Debug.LogWarning("GamesData.csv not found at Resources/Games/GamesData.csv");
                return result;
            }

            using (StringReader reader = new StringReader(csv.text))
            {
                string headerLine = reader.ReadLine();
                if (headerLine == null) return result;

                string[] headers = SplitCsvLine(headerLine);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] fields = SplitCsvLine(line);
                    var row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    for (int i = 0; i < headers.Length && i < fields.Length; i++)
                        row[headers[i].Trim()] = fields[i];

                    result.Add(RowToGameEntry(row));
                }
            }

            result.Sort((a, b) => a.Id.CompareTo(b.Id));

            GenreTagCsvLoader.ApplyTo(result);

            return result;
        }

        private static GameEntry RowToGameEntry(Dictionary<string, string> row)
        {
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

            return new GameEntry(id, title, genre, genres, tags, rating, price, year,
                criticScore, communityAvg, totalRatings, dev, cover, colorHex, storeUrl);
        }

        private static string Get(Dictionary<string, string> row, string key) =>
            row.TryGetValue(key, out var v) ? v : "";

        // Genres/tags are stored semicolon-separated within their CSV cell (a plain
        // comma would be read as a new column), e.g. "Platformer;Indie;Action".
        private static string[] SplitList(string cell) =>
            string.IsNullOrWhiteSpace(cell)
                ? new string[0]
                : cell.Split(';');

        private static int ParseInt(string s) =>
            int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0;

        private static float ParseFloat(string s) =>
            float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0f;

        // Minimal CSV splitter: none of our fields need quoted commas (the only
        // comma-shaped data — genres/tags — uses ';' instead), so a plain split is
        // enough. Handles \r-trimmed lines from Windows-authored CSVs.
        private static string[] SplitCsvLine(string line) => line.TrimEnd('\r').Split(',');
    }
}

// =====END OF TIFFANY SECTION=====
