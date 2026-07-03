using System.Linq;
using UnityEngine.UIElements;

namespace VideoScope.Pages
{
    public class ProfilePageController
    {
        private readonly VisualElement _root;
        private readonly UIManager _manager;
        private UserProfile User => _manager.CurrentUser;
        private UserProfile _draft;

        private bool _editing = false;
        private VisualElement _summaryView, _editView, _recoSection;
        private Button _btnEdit;

        public ProfilePageController(VisualElement root, UIManager manager)
        {
            _root = root;
            _manager = manager;
        }

        public void Bind()
        {
            MakeClickable(_root.Q<Label>("nav-brand"), () => _manager.ShowCatalogue());
            MakeClickable(_root.Q<Label>("nav-catalogue-link"), () => _manager.ShowCatalogue());
            MakeClickable(_root.Q<Label>("nav-signout-link"), () => _manager.SignOut());

            _root.Q<Label>("profile-username").text = User.Username;
            _root.Q<Label>("profile-substats").text =
                $"{User.Ratings.Count} games rated · {User.Completed.Count} completed · {User.Wishlist.Count} in wishlist";

            _summaryView = _root.Q<VisualElement>("summary-view");
            _editView = _root.Q<VisualElement>("edit-view");
            _recoSection = _root.Q<VisualElement>("reco-section");
            _btnEdit = _root.Q<Button>("btn-edit");

            _btnEdit.clicked += () =>
            {
                _editing = !_editing;
                _draft = User.Clone();
                RenderEditMode();
            };

            BuildSummary();
            BuildRecommendations();
            BuildLists();
            RenderEditMode();
        }

        private void RenderEditMode()
        {
            _summaryView.EnableInClassList("hidden", _editing);
            _editView.EnableInClassList("hidden", !_editing);
            _btnEdit.text = _editing ? "Cancel" : "Edit Profile";

            if (_editing) BuildEditForm();
        }

        private void BuildSummary()
        {
            FillChipList(_root.Q<VisualElement>("summary-liked-genres"), User.LikedGenres, "#00e5a0");
            FillChipList(_root.Q<VisualElement>("summary-disliked-genres"), User.DislikedGenres, "#e94560");
            FillChipList(_root.Q<VisualElement>("summary-liked-tags"), User.LikedTags, "#f5c518");
            FillChipList(_root.Q<VisualElement>("summary-fav-creators"), User.FavCreators, "#8b5cf6");
        }

        private void FillChipList(VisualElement container, System.Collections.Generic.List<string> items, string colorHex)
        {
            container.Clear();
            if (items.Count == 0)
            {
                var empty = new Label("None set");
                empty.AddToClassList("summary-empty");
                container.Add(empty);
                return;
            }
            foreach (var item in items)
            {
                var chip = new Label(item);
                chip.AddToClassList("summary-chip");
                if (ColorUtils.TryParse(colorHex, out var c))
                {
                    var faded = c; faded.a = 0.13f;
                    var border = c; border.a = 0.27f;
                    chip.style.backgroundColor = faded;
                    chip.style.borderTopColor = border;
                    chip.style.borderBottomColor = border;
                    chip.style.borderLeftColor = border;
                    chip.style.borderRightColor = border;
                    chip.style.color = c;
                }
                container.Add(chip);
            }
        }

        private void BuildEditForm()
        {
            var likedGenres = _root.Q<VisualElement>("edit-liked-genres");
            var dislikedGenres = _root.Q<VisualElement>("edit-disliked-genres");
            var likedTags = _root.Q<VisualElement>("edit-liked-tags");
            var maxRating = _root.Q<VisualElement>("edit-max-rating");
            var priceLabel = _root.Q<Label>("edit-price-label");
            var priceSlider = _root.Q<Slider>("edit-price-slider");
            var btnSave = _root.Q<Button>("btn-save");

            void RedrawGenres()
            {
                likedGenres.Clear();
                foreach (var g in GameDatabase.Genres)
                {
                    var genre = g;
                    likedGenres.Add(UIHelpers.CreateTag(genre, _draft.LikedGenres.Contains(genre), () =>
                    {
                        ToggleValue(_draft.LikedGenres, genre);
                        RedrawGenres();
                    }));
                }
                dislikedGenres.Clear();
                foreach (var g in GameDatabase.Genres)
                {
                    var genre = g;
                    dislikedGenres.Add(UIHelpers.CreateTag(genre, _draft.DislikedGenres.Contains(genre), () =>
                    {
                        ToggleValue(_draft.DislikedGenres, genre);
                        RedrawGenres();
                    }));
                }
            }
            RedrawGenres();

            void RedrawTags()
            {
                likedTags.Clear();
                foreach (var t in GameDatabase.Tags)
                {
                    var tag = t;
                    likedTags.Add(UIHelpers.CreateTag(tag, _draft.LikedTags.Contains(tag), () =>
                    {
                        ToggleValue(_draft.LikedTags, tag);
                        RedrawTags();
                    }));
                }
            }
            RedrawTags();

            void RedrawRatings()
            {
                maxRating.Clear();
                foreach (var r in GameDatabase.Ratings)
                {
                    var rating = r;
                    maxRating.Add(UIHelpers.CreateTag(rating, _draft.MaxRating == rating, () =>
                    {
                        _draft.MaxRating = rating;
                        RedrawRatings();
                    }));
                }
            }
            RedrawRatings();

            priceLabel.text = $"Max Price: ${_draft.PriceMax:0}";
            priceSlider.SetValueWithoutNotify(_draft.PriceMax);
            priceSlider.RegisterValueChangedCallback(evt =>
            {
                _draft.PriceMax = evt.newValue;
                priceLabel.text = $"Max Price: ${_draft.PriceMax:0}";
            });

            btnSave.clickable = new Clickable(() =>
            {
                _manager.SetUser(_draft);
                _editing = false;
                _manager.ShowProfile();
            });
        }

        private static void ToggleValue(System.Collections.Generic.List<string> list, string value)
        {
            if (list.Contains(value)) list.Remove(value);
            else list.Add(value);
        }

        /// <summary>Label defaults to pickingMode = Ignore in UI Toolkit, so text used as a
        /// nav link needs picking explicitly enabled before it will receive ClickEvents.</summary>
        private static void MakeClickable(VisualElement el, System.Action onClick)
        {
            el.pickingMode = PickingMode.Position;
            el.RegisterCallback<ClickEvent>(_ => onClick());
        }

        private void BuildRecommendations()
        {
            var recos = GameDatabase.Games
                .Where(g => !User.Wishlist.Contains(g.Id) && !User.Completed.Contains(g.Id))
                .Select(g => (game: g, score: ScoreCalculator.CalcUserScore(g, User)))
                .Where(t => t.score >= 75)
                .OrderByDescending(t => t.score)
                .Take(4)
                .ToList();

            _recoSection.EnableInClassList("hidden", recos.Count == 0);
            if (recos.Count == 0) return;

            var grid = _root.Q<VisualElement>("reco-grid");
            grid.Clear();
            foreach (var (game, score) in recos)
            {
                var card = new VisualElement();
                card.AddToClassList("reco-card");

                var icon = new Label(game.Cover);
                icon.AddToClassList("reco-card__icon");
                card.Add(icon);

                var textCol = new VisualElement { style = { flexShrink = 1 } };
                var title = new Label(game.Title);
                title.AddToClassList("reco-card__title");
                var genre = new Label(game.Genre);
                genre.AddToClassList("reco-card__genre");
                var scoreLbl = new Label(score.ToString());
                scoreLbl.AddToClassList("reco-card__score");
                textCol.Add(title);
                textCol.Add(genre);
                textCol.Add(scoreLbl);
                card.Add(textCol);

                card.RegisterCallback<ClickEvent>(_ => _manager.ShowGameDetail(game));
                grid.Add(card);
            }
        }

        private void BuildLists()
        {
            _root.Q<Label>("wishlist-title").text = $"Wishlist ({User.Wishlist.Count})";
            _root.Q<Label>("completed-title").text = $"Completed ({User.Completed.Count})";

            FillGameChips(_root.Q<VisualElement>("wishlist-chips"), _root.Q<Label>("wishlist-empty"), User.Wishlist, "#e94560");
            FillGameChips(_root.Q<VisualElement>("completed-chips"), _root.Q<Label>("completed-empty"), User.Completed, "#00e5a0");
        }

        private void FillGameChips(VisualElement container, Label emptyLabel, System.Collections.Generic.List<int> ids, string colorHex)
        {
            container.Clear();
            emptyLabel.EnableInClassList("hidden", ids.Count != 0);
            if (ids.Count == 0) return;

            ColorUtils.TryParse(colorHex, out var col);
            var borderCol = col; borderCol.a = 0.2f;

            foreach (var id in ids)
            {
                var game = GameDatabase.GetById(id);
                if (game == null) continue;

                var chip = new VisualElement();
                chip.AddToClassList("list-chip");
                chip.style.borderTopColor = borderCol;
                chip.style.borderBottomColor = borderCol;
                chip.style.borderLeftColor = borderCol;
                chip.style.borderRightColor = borderCol;

                var icon = new Label(game.Cover);
                icon.AddToClassList("list-chip__icon");
                chip.Add(icon);

                var textCol = new VisualElement();
                var title = new Label(game.Title);
                title.AddToClassList("list-chip__title");
                var sub = new Label($"User Score: {ScoreCalculator.CalcUserScore(game, User)}");
                sub.AddToClassList("list-chip__sub");
                textCol.Add(title);
                textCol.Add(sub);
                chip.Add(textCol);

                chip.RegisterCallback<ClickEvent>(_ => _manager.ShowGameDetail(game));
                container.Add(chip);
            }
        }
    }
}
