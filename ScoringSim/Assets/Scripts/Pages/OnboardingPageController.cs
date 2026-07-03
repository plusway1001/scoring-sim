using System.Linq;
using UnityEngine.UIElements;

namespace VideoScope.Pages
{
    public class OnboardingPageController
    {
        private readonly VisualElement _root;
        private readonly UIManager _manager;
        private UserProfile _draft;

        private Label _welcomeLabel, _stepTitle, _stepSub;
        private VisualElement _stepContent, _progressRow;
        private Button _btnBack, _btnNext, _btnFinish;

        private int _step = 0;
        private const int StepCount = 4;

        public OnboardingPageController(VisualElement root, UIManager manager)
        {
            _root = root;
            _manager = manager;
            _draft = (manager.CurrentUser ?? new UserProfile()).Clone();
        }

        public void Bind()
        {
            _welcomeLabel = _root.Q<Label>("welcome-label");
            _stepTitle = _root.Q<Label>("step-title");
            _stepSub = _root.Q<Label>("step-sub");
            _stepContent = _root.Q<VisualElement>("step-content");
            _progressRow = _root.Q<VisualElement>("progress-row");
            _btnBack = _root.Q<Button>("btn-back");
            _btnNext = _root.Q<Button>("btn-next");
            _btnFinish = _root.Q<Button>("btn-finish");

            _welcomeLabel.text = $"Welcome, {_draft.Username}";

            _btnBack.clicked += () => { _step = System.Math.Max(0, _step - 1); RenderStep(); };
            _btnNext.clicked += () => { _step = System.Math.Min(StepCount - 1, _step + 1); RenderStep(); };
            _btnFinish.clicked += () => _manager.OnOnboardingComplete(_draft);

            RenderStep();
        }

        private void RenderStep()
        {
            // progress bar
            for (int i = 0; i < StepCount; i++)
            {
                var seg = _progressRow.Q<VisualElement>($"seg-{i}");
                seg.EnableInClassList("progress-seg--active", i <= _step);
            }

            _btnBack.EnableInClassList("hidden", _step == 0);
            _btnNext.EnableInClassList("hidden", _step == StepCount - 1);
            _btnFinish.EnableInClassList("hidden", _step != StepCount - 1);

            _stepContent.Clear();

            switch (_step)
            {
                case 0:
                    _stepTitle.text = "What genres do you love?";
                    _stepSub.text = "We'll boost scores for these.";
                    foreach (var g in GameDatabase.Genres)
                    {
                        var genre = g;
                        _stepContent.Add(UIHelpers.CreateTag(genre, _draft.LikedGenres.Contains(genre), () =>
                        {
                            Toggle(_draft.LikedGenres, genre);
                            RenderStep();
                        }));
                    }
                    break;

                case 1:
                    _stepTitle.text = "Any genres to avoid?";
                    _stepSub.text = "These will lower or hide recommendations.";
                    foreach (var g in GameDatabase.Genres.Where(g => !_draft.LikedGenres.Contains(g)))
                    {
                        var genre = g;
                        _stepContent.Add(UIHelpers.CreateTag(genre, _draft.DislikedGenres.Contains(genre), () =>
                        {
                            Toggle(_draft.DislikedGenres, genre);
                            RenderStep();
                        }));
                    }
                    break;

                case 2:
                    _stepTitle.text = "Pick your favourite tags";
                    _stepSub.text = "Games matching these get a bonus.";
                    foreach (var t in GameDatabase.Tags)
                    {
                        var tag = t;
                        _stepContent.Add(UIHelpers.CreateTag(tag, _draft.LikedTags.Contains(tag), () =>
                        {
                            Toggle(_draft.LikedTags, tag);
                            RenderStep();
                        }));
                    }
                    break;

                case 3:
                    _stepTitle.text = "Set your limits";
                    _stepSub.text = "Games outside these are filtered or penalized.";
                    _stepContent.RemoveFromClassList("tag-row");

                    var ratingLabel = new Label("Max Age Rating");
                    ratingLabel.AddToClassList("price-label");
                    _stepContent.Add(ratingLabel);

                    var ratingRow = new VisualElement();
                    ratingRow.AddToClassList("tag-row");
                    ratingRow.style.marginBottom = 20;
                    foreach (var r in GameDatabase.Ratings)
                    {
                        var rating = r;
                        ratingRow.Add(UIHelpers.CreateTag(rating, _draft.MaxRating == rating, () =>
                        {
                            _draft.MaxRating = rating;
                            RenderStep();
                        }));
                    }
                    _stepContent.Add(ratingRow);

                    var priceLabel = new Label($"Price Range: ${_draft.PriceMin:0} – ${_draft.PriceMax:0}");
                    priceLabel.AddToClassList("price-label");
                    _stepContent.Add(priceLabel);

                    var slider = new Slider(0, 100) { value = _draft.PriceMax };
                    slider.RegisterValueChangedCallback(evt =>
                    {
                        _draft.PriceMax = evt.newValue;
                        priceLabel.text = $"Price Range: ${_draft.PriceMin:0} – ${_draft.PriceMax:0}";
                    });
                    _stepContent.Add(slider);
                    break;
            }

            // Re-add tag-row class for non-final steps (step 3 removes it above).
            if (_step != 3) _stepContent.AddToClassList("tag-row");
        }

        private static void Toggle(System.Collections.Generic.List<string> list, string value)
        {
            if (list.Contains(value)) list.Remove(value);
            else list.Add(value);
        }
    }
}
