using System.Collections.Generic;

namespace VideoScope
{
    /// <summary>Mirrors the `user` object shape used throughout the React app.</summary>
    public class UserProfile
    {
        public string Username = "";
        public List<string> LikedGenres = new List<string>();
        public List<string> DislikedGenres = new List<string>();
        public List<string> LikedTags = new List<string>();
        public string MaxRating = "R18";
        public float PriceMin = 0f;
        public float PriceMax = 100f;
        public List<string> FavCreators = new List<string>();
        public List<int> Wishlist = new List<int>();
        public List<int> Completed = new List<int>();
        public Dictionary<int, int> Ratings = new Dictionary<int, int>();

        public bool HasOnboarded => LikedGenres.Count > 0 || DislikedGenres.Count > 0 || LikedTags.Count > 0;

        public UserProfile Clone()
        {
            return new UserProfile
            {
                Username = Username,
                LikedGenres = new List<string>(LikedGenres),
                DislikedGenres = new List<string>(DislikedGenres),
                LikedTags = new List<string>(LikedTags),
                MaxRating = MaxRating,
                PriceMin = PriceMin,
                PriceMax = PriceMax,
                FavCreators = new List<string>(FavCreators),
                Wishlist = new List<int>(Wishlist),
                Completed = new List<int>(Completed),
                Ratings = new Dictionary<int, int>(Ratings)
            };
        }
    }

    public static class DemoUsers
    {
        public static UserProfile GamerSG()
        {
            var u = new UserProfile
            {
                Username = "GamerSG",
                LikedGenres = new List<string> { "Platformer", "Indie", "RPG" },
                DislikedGenres = new List<string> { "Sports", "Horror" },
                LikedTags = new List<string> { "metroidvania", "difficult", "story-rich" },
                MaxRating = "R16",
                PriceMin = 0,
                PriceMax = 40,
                FavCreators = new List<string> { "Team Cherry" },
                Wishlist = new List<int> { 5, 6 },
                Completed = new List<int>(),
            };
            u.Ratings[1] = 9;
            return u;
        }

        public static UserProfile CasualPlayer()
        {
            return new UserProfile
            {
                Username = "CasualPlayer",
                LikedGenres = new List<string> { "Simulation", "Puzzle", "Sandbox" },
                DislikedGenres = new List<string> { "Horror", "Fighting" },
                LikedTags = new List<string> { "relaxing", "co-op", "pixel-art" },
                MaxRating = "PG13",
                PriceMin = 0,
                PriceMax = 30,
                FavCreators = new List<string> { "ConcernedApe" },
                Wishlist = new List<int> { 4, 10, 12 },
                Completed = new List<int>(),
            };
        }

        /// <summary>Case-insensitive lookup used by the login screen.</summary>
        public static UserProfile Find(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            if (username.Equals("GamerSG", System.StringComparison.OrdinalIgnoreCase)) return GamerSG();
            if (username.Equals("CasualPlayer", System.StringComparison.OrdinalIgnoreCase)) return CasualPlayer();
            return null;
        }
    }
}
