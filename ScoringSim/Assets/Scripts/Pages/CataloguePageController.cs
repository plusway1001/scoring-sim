using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace GameScope.Pages
{
    public class CataloguePageController
    {
        private readonly VisualElement _root;
        private readonly UIManager _manager;
        private UserProfile User => _manager.CurrentUser;

        private TextField _search;
        private ThemedDropdown _mainGenreDropdown, _subGenreDropdown, _tagDropdown, _ratingDropdown, _sortDropdown;
        private Label _resultsCount, _emptyLabel, _navUsernameLink, _navProfileLink, _navSignInLink;
        private VisualElement _heroPanel, _legendRow, _grid;
        private Label _heroUsername, _heroSub, _heroMatches, _heroWishlistCount;

        // NEW: Main Genre / Sub Genre / Tags replace what used to be a single genre
        // dropdown, so the catalogue can filter on the richer Resources/Data/GenreTagData.csv
        // taxonomy (see GameEntry.MainGenres/SubGenres) as well as the existing flavor tags.
        private readonly List<string> _mainGenreOptions;
        private readonly List<string> _subGenreOptions;
        private readonly List<string> _tagOptions;
        private readonly List<string> _ratingOptions;
        private readonly List<string> _sortOptions;

        // NEW: distinct default-option labels so the four filter dropdowns don't all
        // just say the same generic "All" (previously indistinguishable at a glance —
        // see Refresh() below, where these same strings are the "no filter" sentinel).
        private const string AllMainGenre = "All Main Genre";
        private const string AllSubGenre = "All Sub Genre";
        private const string AllTags = "All Tags";
        private const string AllRatings = "All Ratings";

        public CataloguePageController(VisualElement root, UIManager manager)
        {
            _root = root;
            _manager = manager;
            _mainGenreOptions = new List<string> { AllMainGenre }.Concat(GameDatabase.MainGenreOptions).ToList();
            _subGenreOptions = new List<string> { AllSubGenre }.Concat(GameDatabase.SubGenreOptions).ToList();
            _tagOptions = new List<string> { AllTags }.Concat(GameDatabase.Tags).ToList();
            _ratingOptions = new List<string> { AllRatings }.Concat(GameDatabase.Ratings).ToList();
            _sortOptions = User != null
                ? new List<string> { "Sort: Your Score", "Sort: General Score", "Sort: Price", "Sort: Newest" }
                : new List<string> { "Sort: General Score", "Sort: Price", "Sort: Newest" };
        }

        public void Bind()
        {
            var navBrand = _root.Q<Label>("nav-brand");
            MakeClickable(navBrand, () => _manager.ShowCatalogue());

            _navProfileLink = _root.Q<Label>("nav-profile-link");
            _navUsernameLink = _root.Q<Label>("nav-username-link");
            _navSignInLink = _root.Q<Label>("nav-signin-link");
            MakeClickable(_navProfileLink, () => _manager.ShowProfile());
            MakeClickable(_navUsernameLink, () => _manager.ShowProfile());
            MakeClickable(_navSignInLink, () => _manager.ShowLogin());

            bool loggedIn = User != null;
            _navProfileLink.EnableInClassList("hidden", !loggedIn);
            _navUsernameLink.EnableInClassList("hidden", !loggedIn);
            _navSignInLink.EnableInClassList("hidden", loggedIn);
            if (loggedIn) _navUsernameLink.text = User.Username;

            _search = _root.Q<TextField>("search-field");
            // NEW: real in-field placeholder + guaranteed-visible white cursor.
            // See UIHelpers.cs (AddPlaceholder / ForceWhiteText) for what/why.
            UIHelpers.AddPlaceholder(_search, "Search games...");
            UIHelpers.ForceWhiteText(_search);
            // NEW: swap the plain UXML placeholders for real ThemedDropdown controls
            // (see Scripts/ThemedDropdown.cs) — replaces Unity's native DropdownField,
            // whose open list can't be restyled with USS.
            _mainGenreDropdown = ReplaceWithThemedDropdown(_root.Q<VisualElement>("main-genre-dropdown"));
            _subGenreDropdown = ReplaceWithThemedDropdown(_root.Q<VisualElement>("sub-genre-dropdown"));
            _tagDropdown = ReplaceWithThemedDropdown(_root.Q<VisualElement>("tag-dropdown"));
            _ratingDropdown = ReplaceWithThemedDropdown(_root.Q<VisualElement>("rating-dropdown"));
            _sortDropdown = ReplaceWithThemedDropdown(_root.Q<VisualElement>("sort-dropdown"));
            _resultsCount = _root.Q<Label>("results-count");
            _emptyLabel = _root.Q<Label>("empty-label");
            _heroPanel = _root.Q<VisualElement>("hero-panel");
            _legendRow = _root.Q<VisualElement>("legend-row");
            _grid = _root.Q<VisualElement>("game-grid");

            _heroUsername = _root.Q<Label>("hero-username");
            _heroSub = _root.Q<Label>("hero-sub");
            _heroMatches = _root.Q<Label>("hero-matches");
            _heroWishlistCount = _root.Q<Label>("hero-wishlist-count");

            _mainGenreDropdown.choices = _mainGenreOptions;
            _mainGenreDropdown.index = 0;
            _subGenreDropdown.choices = _subGenreOptions;
            _subGenreDropdown.index = 0;
            _tagDropdown.choices = _tagOptions;
            _tagDropdown.index = 0;
            _ratingDropdown.choices = _ratingOptions;
            _ratingDropdown.index = 0;
            _sortDropdown.choices = _sortOptions;
            _sortDropdown.index = 0;

            _search.RegisterValueChangedCallback(_ => Refresh());
            _mainGenreDropdown.RegisterValueChangedCallback(_ => Refresh());
            _subGenreDropdown.RegisterValueChangedCallback(_ => Refresh());
            _tagDropdown.RegisterValueChangedCallback(_ => Refresh());
            _ratingDropdown.RegisterValueChangedCallback(_ => Refresh());
            _sortDropdown.RegisterValueChangedCallback(_ => Refresh());

            _heroPanel.EnableInClassList("hidden", !loggedIn);
            _legendRow.EnableInClassList("hidden", !loggedIn);

            Refresh();
        }

        private void Refresh()
        {
            var filtered = GameDatabase.Games.AsEnumerable();

            string q = (_search.value ?? "").ToLowerInvariant();
            if (!string.IsNullOrEmpty(q))
                filtered = filtered.Where(g => g.Title.ToLowerInvariant().Contains(q));

            if (_mainGenreDropdown.value != AllMainGenre)
                filtered = filtered.Where(g => g.MainGenres.Contains(_mainGenreDropdown.value));

            if (_subGenreDropdown.value != AllSubGenre)
                filtered = filtered.Where(g => g.SubGenres.Contains(_subGenreDropdown.value));

            if (_tagDropdown.value != AllTags)
                filtered = filtered.Where(g => g.Tags.Contains(_tagDropdown.value));

            if (_ratingDropdown.value != AllRatings)
                filtered = filtered.Where(g => g.Rating == _ratingDropdown.value);

            var list = filtered.ToList();

            switch (_sortDropdown.value)
            {
                case "Sort: Your Score":
                    list.Sort((a, b) => ScoreCalculator.CalcUserScore(b, User).CompareTo(ScoreCalculator.CalcUserScore(a, User)));
                    break;
                case "Sort: General Score":
                    list.Sort((a, b) => ScoreCalculator.CalcGeneralScore(b).CompareTo(ScoreCalculator.CalcGeneralScore(a)));
                    break;
                case "Sort: Price":
                    list.Sort((a, b) => a.Price.CompareTo(b.Price));
                    break;
                case "Sort: Newest":
                    list.Sort((a, b) => b.Year.CompareTo(a.Year));
                    break;
            }

            _resultsCount.text = $"{list.Count} games";
            _emptyLabel.EnableInClassList("hidden", list.Count != 0);

            if (User != null)
            {
                _heroUsername.text = User.Username;
                _heroSub.text = $"{User.LikedGenres.Count} liked genres · {User.Wishlist.Count} in wishlist cart · {User.Ratings.Count} games rated";
                _heroMatches.text = list.Count(g => ScoreCalculator.CalcUserScore(g, User) >= 85).ToString();
                _heroWishlistCount.text = User.Wishlist.Count.ToString();
            }

            _grid.Clear();
            foreach (var game in list)
                _grid.Add(BuildCard(game));
        }

        private VisualElement BuildCard(GameEntry game)
        {
            var card = _manager.gameCardTemplate.CloneTree();
            var root = card.Q<VisualElement>("card-root");

            var icon = card.Q<Label>("card-icon");
            icon.text = game.Cover;
            if (ColorUtils.TryParse(game.ColorHex, out var col)) icon.style.backgroundColor = col;

            UIHelpers.BuildScoreBadge(card.Q<VisualElement>("card-general-score"), ScoreCalculator.CalcGeneralScore(game), "score-badge--sm");

            var userScoreCol = card.Q<VisualElement>("card-user-score");
            if (User != null)
                UIHelpers.BuildScoreBadge(userScoreCol, ScoreCalculator.CalcUserScore(game, User), "score-badge--sm");
            else
                userScoreCol.style.display = DisplayStyle.None;

            card.Q<Label>("card-title").text = game.Title;
            card.Q<Label>("card-meta").text = $"{game.Genre} · {game.Year} · ${game.Price:0.00}";
            card.Q<Label>("card-rating-chip").text = game.Rating;

            var tag1 = card.Q<Label>("card-tag-1");
            var tag2 = card.Q<Label>("card-tag-2");
            tag1.text = game.Tags.Length > 0 ? game.Tags[0] : "";
            tag1.style.display = game.Tags.Length > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            tag2.text = game.Tags.Length > 1 ? game.Tags[1] : "";
            tag2.style.display = game.Tags.Length > 1 ? DisplayStyle.Flex : DisplayStyle.None;

            root.RegisterCallback<ClickEvent>(_ => _manager.ShowGameDetail(game));

            var wishlistRow = card.Q<VisualElement>("card-wishlist-row");
            if (User != null)
            {
                wishlistRow.RemoveFromClassList("hidden");
                bool inWishlist = User.Wishlist.Contains(game.Id);
                UpdateWishlistRow(wishlistRow, inWishlist);
                wishlistRow.RegisterCallback<ClickEvent>(evt =>
                {
                    evt.StopPropagation();
                    if (User.Wishlist.Contains(game.Id)) User.Wishlist.Remove(game.Id);
                    else User.Wishlist.Add(game.Id);
                    Refresh();
                });
            }

            return card;
        }

        /// <summary>NEW: swaps a plain placeholder VisualElement (declared
        /// in UXML with a name/class so layout CSS still applies) for a real
        /// ThemedDropdown at the same position in the tree.</summary>
        private static ThemedDropdown ReplaceWithThemedDropdown(VisualElement placeholder)
        {
            var dropdown = new ThemedDropdown { name = placeholder.name };
            foreach (var cls in placeholder.GetClasses()) dropdown.AddToClassList(cls);
            var parent = placeholder.parent;
            int idx = parent.IndexOf(placeholder);
            parent.RemoveAt(idx);
            parent.Insert(idx, dropdown);
            return dropdown;
        }

        /// <summary>Labels default to pickingMode = Ignore in UI Toolkit, so text used as a
        /// nav link needs picking explicitly enabled before it will receive ClickEvents.</summary>
        private static void MakeClickable(VisualElement el, System.Action onClick)
        {
            el.pickingMode = PickingMode.Position;
            el.RegisterCallback<ClickEvent>(_ => onClick());
        }

        private static void UpdateWishlistRow(VisualElement row, bool inWishlist)
        {
            row.EnableInClassList("game-card__wishlist--active", inWishlist);
            row.Q<Label>("card-heart").text = inWishlist ? "♥" : "♡";
            // "Wishlist Cart": games you've added are the ones you intend to buy — see the
            // Wishlist Cart section on the Profile page and the "Go to Store" button there
            // and on the Game Detail page (adapted from main2's ScoreDisplayUI.OpenWebsite()).
            row.Q<Label>("card-wishlist-text").text = inWishlist ? "In Wishlist Cart" : "Add to Wishlist Cart";
        }
    }
}
