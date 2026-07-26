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

        public static readonly List<GameEntry> Games = new List<GameEntry>
        {
            new GameEntry(1,"Hollow Knight","Platformer", new[]{"Platformer","Indie","Action"}, new[]{"metroidvania","difficult","atmospheric","pixel-art"}, "G", 13.50f, 2017, 90, 9.1f, 28000, "Team Cherry", "\U0001FAB2", "#1a1a3e", "https://store.steampowered.com/app/367520/Hollow_Knight/"),
            new GameEntry(2,"Elden Ring","RPG", new[]{"RPG","Action","Adventure"}, new[]{"open-world","difficult","fantasy","atmospheric"}, "R16", 79.90f, 2022, 96, 9.3f, 45000, "FromSoftware", "\u2694\uFE0F", "#2a1a0e", "https://store.steampowered.com/app/1245620/ELDEN_RING/"),
            new GameEntry(3,"Dead Space","Horror", new[]{"Horror","Action","Shooter"}, new[]{"sci-fi","atmospheric","difficult","story-rich"}, "R18", 59.90f, 2023, 89, 8.8f, 18000, "Motive Studio", "\U0001F47E", "#1a0a0a", "https://store.steampowered.com/app/1693980/Dead_Space/"),
            new GameEntry(4,"Stardew Valley","Simulation", new[]{"Simulation","Indie","RPG"}, new[]{"relaxing","sandbox","pixel-art","co-op"}, "G", 19.90f, 2016, 90, 9.5f, 52000, "ConcernedApe", "\U0001F33E", "#1a2e1a", "https://store.steampowered.com/app/413150/Stardew_Valley/"),
            new GameEntry(5,"Hades","Action", new[]{"Action","Indie","RPG"}, new[]{"roguelike","story-rich","difficult","atmospheric"}, "R16", 29.90f, 2020, 93, 9.2f, 38000, "Supergiant Games", "\U0001F531", "#2e1a0a", "https://store.steampowered.com/app/1145360/Hades/"),
            new GameEntry(6,"Celeste","Platformer", new[]{"Platformer","Indie"}, new[]{"difficult","pixel-art","story-rich","atmospheric"}, "G", 19.90f, 2018, 94, 9.0f, 22000, "Maddy Thorson", "\u26F0\uFE0F", "#1a0a2e", "https://store.steampowered.com/app/504230/Celeste/"),
            new GameEntry(7,"FIFA 24","Sports", new[]{"Sports"}, new[]{"multiplayer","co-op","anime"}, "G", 69.90f, 2023, 73, 6.8f, 31000, "EA Sports", "\u26BD", "#0a1a2e", "https://www.ea.com/games/ea-sports-fc/fc-24"),
            new GameEntry(8,"Resident Evil 4","Horror", new[]{"Horror","Action","Shooter"}, new[]{"story-rich","atmospheric","difficult"}, "R18", 59.90f, 2023, 93, 9.1f, 24000, "Capcom", "\U0001F9DF", "#1a0e08", "https://store.steampowered.com/app/2050650/Resident_Evil_4/"),
            new GameEntry(9,"Disco Elysium","RPG", new[]{"RPG","Adventure","Indie"}, new[]{"story-rich","atmospheric","fantasy"}, "R18", 39.90f, 2019, 97, 9.3f, 19000, "ZA/UM", "\U0001F575\uFE0F", "#0e1a1a", "https://store.steampowered.com/app/632470/Disco_Elysium__The_Final_Cut/"),
            new GameEntry(10,"Tetris Effect","Puzzle", new[]{"Puzzle","Indie"}, new[]{"relaxing","atmospheric","pixel-art"}, "G", 29.90f, 2018, 93, 9.0f, 12000, "Enhance", "\U0001F7E6", "#0a0a2e", "https://store.steampowered.com/app/1003590/Tetris_Effect_Connected/"),
            new GameEntry(11,"Street Fighter 6","Fighting", new[]{"Fighting","Action"}, new[]{"multiplayer","co-op","story-rich"}, "PG13", 59.90f, 2023, 92, 8.9f, 16000, "Capcom", "\U0001F94A", "#2e0a0a", "https://store.steampowered.com/app/1364780/Street_Fighter_6/"),
            new GameEntry(12,"Minecraft","Sandbox", new[]{"Sandbox","Simulation","Adventure"}, new[]{"open-world","co-op","relaxing","sandbox"}, "G", 29.90f, 2011, 93, 9.0f, 65000, "Mojang", "\u26CF\uFE0F", "#1e2e0e", "https://www.minecraft.net/en-us/store/minecraft-java-bedrock-edition-pc"),
        };
        // Note: these are illustrative real storefront links for the demo catalogue, not
        // sourced from main2 (main2's websiteURL field is populated per-game from JSON,
        // see Data Loader/GameLoader.cs, which GameScope's static-list catalogue doesn't use).

        public static GameEntry GetById(int id)
        {
            return Games.Find(g => g.Id == id);
        }
    }
}
