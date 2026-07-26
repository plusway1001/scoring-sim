// ADAPTED FROM main2/Assets/Scripts/Data Loader/GenreTagDataLoader.cs — same CSV
// format and parsing rules (two tables in one file: a "Genre Name" header row
// followed by main-genre rows, then a "Game Tag" header row followed by
// sub-genre rows; each row is TRUE/FALSE membership per game column, matched by
// title). main2's own version built a separate, parallel GameGenreData list that
// was never actually merged back into the real game objects; this version merges
// the same TRUE/FALSE data straight into GameEntry.MainGenres/SubGenres instead,
// as a static utility rather than a MonoBehaviour, since GameCatalogueLoader
// calls it once right after building the catalogue from JSON.
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameScope
{
    public static class GenreTagCsvLoader
    {
        public static void ApplyTo(List<GameEntry> games)
        {
            TextAsset csv = Resources.Load<TextAsset>("Data/GenreTagData");
            if (csv == null) return;

            using (StringReader reader = new StringReader(csv.text))
            {
                string line;
                string[] headers = null;
                bool isGenre = false, isTag = false;

                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] fields = line.Split(',');
                    string first = fields[0].Trim().Trim('\uFEFF');

                    if (first == "Genre Name")
                    {
                        headers = fields;
                        isGenre = true;
                        isTag = false;
                        continue;
                    }

                    if (first == "Game Tag")
                    {
                        headers = fields;
                        isGenre = false;
                        isTag = true;
                        continue;
                    }

                    if (headers == null) continue;

                    string category = first;
                    for (int i = 1; i < fields.Length && i < headers.Length; i++)
                    {
                        string title = headers[i].Trim();
                        if (!ParseBool(fields[i])) continue;

                        GameEntry game = games.Find(g =>
                            string.Equals(g.Title?.Trim(), title, StringComparison.OrdinalIgnoreCase));
                        if (game == null) continue;

                        if (isGenre)
                        {
                            if (!game.MainGenres.Contains(category)) game.MainGenres.Add(category);
                        }
                        else if (isTag)
                        {
                            if (!game.SubGenres.Contains(category)) game.SubGenres.Add(category);
                        }
                    }
                }
            }
        }

        private static bool ParseBool(string value)
        {
            string cleaned = value.Trim().ToLower();
            return cleaned == "true" || cleaned == "y" || cleaned == "yes" || cleaned == "1";
        }
    }
}
