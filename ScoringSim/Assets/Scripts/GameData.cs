using System.Collections.Generic;

namespace GameScope
{
    /// <summary>
    /// Plain data record for a single game. Mirrors the `GAMES` array
    /// objects from the original React prototype.
    /// </summary>
    public class GameEntry
    {
        public int Id;
        public string Title;
        public string Genre;
        public string[] Genres;
        public string[] Tags;
        public string Rating;       // "G" | "PG13" | "R16" | "R18"
        public float Price;
        public int Year;
        public int CriticScore;     // 0-100
        public float CommunityAvg;  // 0-10
        public int TotalRatings;
        public string Dev;
        public string Cover;        // emoji glyph used as the "cover art"
        public string ColorHex;     // background tile color, e.g. "#1a1a3e"

        // === CODE FROM main2/Assets/Scripts/Data/GameData.cs === LINE 13 (`public string websiteURL;`) ===
        // Renamed to StoreUrl to match GameScope's naming, otherwise the same idea: the
        // external page a player is sent to when they want to actually buy the game.
        // Used by the "Go to Store" button (see GameDetailPageController / ProfilePageController),
        // itself adapted from main2's ScoreDisplayUI.OpenWebsite() (UI_Display_Test/ScoreDisplayUI.cs, line 561).
        public string StoreUrl;

        // NEW: main-genre / sub-genre membership, loaded from Resources/Data/GenreTagData.csv
        // (see Scripts/GenreTagCsvLoader.cs, adapted from main2's GenreTagDataLoader.cs). Kept
        // separate from the original Genre/Genres/Tags fields above, which the scoring system
        // and onboarding preferences still use unchanged — these two are purely for the
        // Catalogue page's Main Genre / Sub Genre filter dropdowns. A game can belong to
        // several of each (e.g. Elden Ring: Main Genres = Fantasy, Adventure, Role-Playing,
        // Survival, PVP).
        public List<string> MainGenres = new List<string>();
        public List<string> SubGenres = new List<string>();

        public GameEntry(int id, string title, string genre, string[] genres, string[] tags,
            string rating, float price, int year, int criticScore, float communityAvg,
            int totalRatings, string dev, string cover, string colorHex, string storeUrl = null)
        {
            StoreUrl = storeUrl;
            Id = id;
            Title = title;
            Genre = genre;
            Genres = genres;
            Tags = tags;
            Rating = rating;
            Price = price;
            Year = year;
            CriticScore = criticScore;
            CommunityAvg = communityAvg;
            TotalRatings = totalRatings;
            Dev = dev;
            Cover = cover;
            ColorHex = colorHex;
        }
    }

    /// <summary>Static catalogue + lookup lists, 1:1 with the JS constants.</summary>
    public static class GameDatabase
    {
        public static readonly string[] Genres =
        {
            "Action","Adventure","RPG","Platformer","Horror","Puzzle","Strategy",
            "Simulation","Sports","Fighting","Shooter","Racing","Indie","Sandbox","Visual Novel"
        };

        public static readonly string[] Tags =
        {
            "open-world","co-op","story-rich","difficult","atmospheric","multiplayer",
            "pixel-art","roguelike","metroidvania","relaxing","sandbox","anime","sci-fi","fantasy","stealth"
        };

        public static readonly string[] Ratings = { "G", "PG13", "R16", "R18" };

        // NEW: the two option lists for the Catalogue page's Main Genre / Sub Genre filter
        // dropdowns — matches the "Genre Name" / "Game Tag" row labels in
        // Resources/Data/GenreTagData.csv exactly (see GenreTagCsvLoader.cs).
        public static readonly string[] MainGenreOptions =
        {
            "Horror", "PVP", "Adventure", "Sports", "Role-Playing", "Fantasy", "Survival"
        };

        public static readonly string[] SubGenreOptions =
        {
            "Platformer", "Action", "Simulation", "First-Person", "Multiplayer", "2D", "3D"
        };

        // NEW: Games used to be a hardcoded list literal here. It's now loaded once (and
        // cached) from Resources/Games/*.json via GameCatalogueLoader — see that file for
        // why, and Resources/Data/GenreTagData.csv + GenreTagCsvLoader.cs for how
        // MainGenres/SubGenres get filled in on top of what's in each game's JSON.
        private static List<GameEntry> _games;
        public static List<GameEntry> Games => _games ??= GameCatalogueLoader.LoadAll();

        public static GameEntry GetById(int id)
        {
            return Games.Find(g => g.Id == id);
        }
    }
}
