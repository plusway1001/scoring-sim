using System;
using System.Linq;
using UnityEngine.UIElements;

namespace VideoScope.Pages
{
    public class GameDetailPageController
    {
        private readonly VisualElement _root;
        private readonly UIManager _manager;
        private readonly GameEntry _game;
        private UserProfile User => _manager.CurrentUser;

        private static readonly string[] RatingOrder = { "G", "PG13", "R16", "R18" };

        private Button _btnWishlist, _btnCompleted;
        private VisualElement _ratingRow;
        private Label _ratingConfirm;
        private int _currentRating;

        public GameDetailPageController(VisualElement root, UIManager manager, GameEntry game)
        {
            _root = root;
            _manager = manager;
            _game = game;
        }

        public void Bind()
        {
            _root.Q<Button>("btn-back").clicked += () => _manager.ShowCatalogue();

            int generalScore = ScoreCalculator.CalcGeneralScore(_game);
            int userScore = ScoreCalculator.CalcUserScore(_game, User);

            // ---- Hero ----
            var icon = _root.Q<Label>("hero-icon");
            icon.text = _game.Cover;
            if (ColorUtils.TryParse(_game.ColorHex, out var col)) icon.style.backgroundColor = col;

            _root.Q<Label>("hero-eyebrow").text = $"{_game.Genre} · {_game.Year}";
            _root.Q<Label>("hero-title").text = _game.Title;
            _root.Q<Label>("hero-byline").text = $"by {_game.Dev} · {_game.Rating} · ${_game.Price:0.00}";

            var heroTags = _root.Q<VisualElement>("hero-tags");
            foreach (var t in _game.Tags)
            {
                var chip = new Label(t);
                chip.AddToClassList("game-card__chip");
                heroTags.Add(chip);
            }

            UIHelpers.BuildScoreBadge(_root.Q<VisualElement>("hero-general-score"), generalScore, "score-badge--lg", "General");
            var userScoreCol = _root.Q<VisualElement>("hero-user-score");
            if (User != null)
                UIHelpers.BuildScoreBadge(userScoreCol, userScore, "score-badge--lg", "Your Score");
            else
                userScoreCol.style.display = DisplayStyle.None;

            // ---- General score breakdown ----
            var breakdown = _root.Q<VisualElement>("general-breakdown");
            double vol = Math.Min(Math.Log(_game.TotalRatings) / Math.Log(70000) * 15, 15);
            double nostalgia = Math.Max(0, (2024 - _game.Year) / 30.0) * 10;

            AddStatRow(breakdown, "Critic Score", "40%", _game.CriticScore);
            AddStatRow(breakdown, "Community Average", "35%", (int)Math.Round(_game.CommunityAvg * 10));
            AddStatRow(breakdown, "Volume Bonus", "15%", (int)Math.Round(vol));
            AddStatRow(breakdown, "Nostalgia Factor", "10%", (int)Math.Round(nostalgia));
            _root.Q<Label>("general-total").text = generalScore.ToString();

            // ---- User score modifiers ----
            var userPanel = _root.Q<VisualElement>("user-panel");
            if (User != null)
            {
                userPanel.RemoveFromClassList("hidden");
                var modifierList = _root.Q<VisualElement>("modifier-list");
                BuildModifiers(modifierList);
                _root.Q<Label>("user-total").text = userScore.ToString();
            }

            // ---- Actions ----
            var actionRow = _root.Q<VisualElement>("action-row");
            if (User != null)
            {
                actionRow.RemoveFromClassList("hidden");
                _btnWishlist = _root.Q<Button>("btn-wishlist");
                _btnCompleted = _root.Q<Button>("btn-completed");
                RefreshActionButtons();

                _btnWishlist.clicked += () =>
                {
                    if (User.Wishlist.Contains(_game.Id)) User.Wishlist.Remove(_game.Id);
                    else User.Wishlist.Add(_game.Id);
                    RefreshActionButtons();
                };
                _btnCompleted.clicked += () =>
                {
                    if (User.Completed.Contains(_game.Id)) User.Completed.Remove(_game.Id);
                    else User.Completed.Add(_game.Id);
                    RefreshActionButtons();
                };
            }

            // ---- Rating ----
            var ratePanel = _root.Q<VisualElement>("rate-panel");
            if (User != null)
            {
                ratePanel.RemoveFromClassList("hidden");
                _root.Q<Label>("rate-sub").text =
                    $"Your rating directly sets your User Score and may add {_game.Dev} to your favourite creators.";
                _ratingRow = _root.Q<VisualElement>("rating-row");
                _ratingConfirm = _root.Q<Label>("rating-confirm");
                User.Ratings.TryGetValue(_game.Id, out _currentRating);
                BuildRatingButtons();
            }
        }

        private void AddStatRow(VisualElement container, string label, string weight, int value)
        {
            var row = new VisualElement();
            row.AddToClassList("stat-row");

            var labels = new VisualElement();
            labels.AddToClassList("stat-row__labels");

            var lbl = new Label($"{label} ");
            lbl.AddToClassList("stat-row__label");
            var weightLbl = new Label($"({weight})");
            weightLbl.AddToClassList("stat-row__weight");
            var labelWrap = new VisualElement { style = { flexDirection = FlexDirection.Row } };
            labelWrap.Add(lbl);
            labelWrap.Add(weightLbl);

            var valLbl = new Label(value.ToString());
            valLbl.AddToClassList("stat-row__value");

            labels.Add(labelWrap);
            labels.Add(valLbl);
            row.Add(labels);

            var barBg = new VisualElement();
            barBg.AddToClassList("stat-row__bar-bg");
            var barFill = new VisualElement();
            barFill.AddToClassList("stat-row__bar-fill");
            barFill.style.width = new StyleLength(new Length(Math.Clamp(value, 0, 100), LengthUnit.Percent));
            barBg.Add(barFill);
            row.Add(barBg);

            container.Add(row);
        }

        private void BuildModifiers(VisualElement container)
        {
            bool likedGenre = User.LikedGenres.Contains(_game.Genre);
            bool dislikedGenre = User.DislikedGenres.Contains(_game.Genre);
            int tagMatches = _game.Tags.Count(t => User.LikedTags.Contains(t));
            bool priceFit = _game.Price >= User.PriceMin && _game.Price <= User.PriceMax;
            bool favCreator = User.FavCreators.Contains(_game.Dev);
            bool blocked = Array.IndexOf(RatingOrder, _game.Rating) > Array.IndexOf(RatingOrder, User.MaxRating);

            AddModifierRow(container, "Genre Match",
                likedGenre ? "+20" : dislikedGenre ? "\u221220" : "0",
                likedGenre || dislikedGenre, likedGenre ? "good" : dislikedGenre ? "bad" : "muted");

            AddModifierRow(container, "Tag Affinity", $"+{Math.Min(tagMatches * 2, 10)}",
                tagMatches > 0, "good");

            AddModifierRow(container, "Age Rating", blocked ? "BLOCKED" : "OK", true, blocked ? "bad" : "good");

            AddModifierRow(container, "Price Fit", priceFit ? "+5" : "0", priceFit, "good");

            AddModifierRow(container, "Fav Creator", favCreator ? "+10" : "0", favCreator, "good");
        }

        private void AddModifierRow(VisualElement container, string label, string value, bool active, string tone)
        {
            var row = new VisualElement();
            row.AddToClassList("modifier-row");

            var lbl = new Label(label);
            lbl.AddToClassList("modifier-row__label");

            var val = new Label(value);
            val.AddToClassList("modifier-row__value");
            val.AddToClassList(active ? $"modifier-row__value--{tone}" : "modifier-row__value--muted");

            row.Add(lbl);
            row.Add(val);
            container.Add(row);
        }

        private void RefreshActionButtons()
        {
            bool inWishlist = User.Wishlist.Contains(_game.Id);
            _btnWishlist.text = inWishlist ? "♥ In Wishlist" : "♡ Add to Wishlist";
            _btnWishlist.EnableInClassList("btn-toggle--active", inWishlist);

            bool inCompleted = User.Completed.Contains(_game.Id);
            _btnCompleted.text = inCompleted ? "✓ Completed" : "Mark as Completed";
            _btnCompleted.EnableInClassList("btn-toggle--active-green", inCompleted);
        }

        private void BuildRatingButtons()
        {
            _ratingRow.Clear();
            for (int n = 1; n <= 10; n++)
            {
                int value = n;
                var btn = new Button { text = value.ToString() };
                btn.AddToClassList("rating-btn");
                if (_currentRating >= value) btn.AddToClassList("rating-btn--active");
                btn.clicked += () => SubmitRating(value);
                _ratingRow.Add(btn);
            }
            UpdateRatingConfirm();
        }

        private void SubmitRating(int rating)
        {
            _currentRating = rating;
            User.Ratings[_game.Id] = rating;
            if (rating >= 8 && !User.FavCreators.Contains(_game.Dev))
                User.FavCreators.Add(_game.Dev);

            BuildRatingButtons();

            // Rating changes the user score panel + modifiers; simplest correct
            // refresh is to redraw this whole detail page.
            _manager.ShowGameDetail(_game);
        }

        private void UpdateRatingConfirm()
        {
            if (_currentRating > 0)
            {
                _ratingConfirm.text = $"✓ You rated this {_currentRating}/10 — User Score updated to {_currentRating * 10}";
                _ratingConfirm.RemoveFromClassList("hidden");
            }
            else
            {
                _ratingConfirm.AddToClassList("hidden");
            }
        }
    }
}
