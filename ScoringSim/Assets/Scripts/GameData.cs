using System.Collections.Generic;

namespace GameScope
{
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

        // === CODE FROM main2/Assets/Scripts/Data/GameData.cs ===
        // external page a player is sent to when they want to actually buy the game.
        // Used by the "Go to Store" button (see GameDetailPageController / ProfilePageController),
        // itself adapted from main2's ScoreDisplayUI.OpenWebsite() (UI_Display_Test/ScoreDisplayUI.cs, line 561).
        public string StoreUrl;

        // =====CHLOE SECTION===== (classification of games into their genres/tags)
        public List<string> MainGenres = new List<string>();
        public List<string> SubGenres = new List<string>();
        // =====END OF CHLOE SECTION=====

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

        // =====CHLOE SECTION===== 
        public static readonly string[] MainGenreOptions =
        {
            "Horror", "PVP", "Adventure", "Sports", "Role-Playing", "Fantasy", "Survival"
        };

        public static readonly string[] SubGenreOptions =
        {
            "Platformer", "Action", "Simulation", "First-Person", "Multiplayer", "2D", "3D"
        };
        // =====END OF CHLOE SECTION=====

        // =====TIFFANY SECTION===== (game data loading)
        private static List<GameEntry> _games;
        public static List<GameEntry> Games => _games ??= GameCatalogueLoader.LoadAll();
        // =====END OF TIFFANY SECTION=====

        public static GameEntry GetById(int id)
        {
            return Games.Find(g => g.Id == id);
        }
    }
}
