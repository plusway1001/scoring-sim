// =====ZAHRA SECTION=====
// === ADAPTED FROM main2/Assets/Scripts/CommunityTagSystem/CommunityTagManager.cs === LINE 1-218 ===
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameScope.Pages
{
    public class CommunityTagBoardController
    {
        private static readonly Dictionary<int, TagDatabase> _boards = new Dictionary<int, TagDatabase>();

        private readonly VisualElement _root;
        private readonly GameEntry _game;
        private TagDatabase _tagDatabase;

        private TextField _tagInput;
        private Button _btnSubmit;
        private VisualElement _officialRow;
        private VisualElement _communityRow;

        public CommunityTagBoardController(VisualElement root, GameEntry game)
        {
            _root = root;
            _game = game;
        }

        public void Bind()
        {
            _tagInput = _root.Q<TextField>("tag-input");
            UIHelpers.ForceWhiteText(_tagInput);
            UIHelpers.AddPlaceholder(_tagInput, "Propose a tag...");
            _btnSubmit = _root.Q<Button>("btn-submit-tag");
            _officialRow = _root.Q<VisualElement>("official-tag-row");
            _communityRow = _root.Q<VisualElement>("community-tag-row");

            if (!_boards.TryGetValue(_game.Id, out _tagDatabase))
            {
                _tagDatabase = new TagDatabase();
                // === CODE FROM main2/Assets/Scripts/CommunityTagSystem/CommunityTagManager.cs === LINE 116-123 (Start) ===
                foreach (var t in _game.Tags)
                    _tagDatabase.communityTags.Add(new CommunityTag(t, true));
                _boards[_game.Id] = _tagDatabase;
            }

            _btnSubmit.clicked += SubmitTag;
            _tagInput.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                    SubmitTag();
            });

            RefreshCommunityTags();
        }

        // === CODE FROM main2/Assets/Scripts/CommunityTagSystem/CommunityTagManager.cs === LINE 128-172 (SubmitTag) ===
        public void SubmitTag()
        {
            string newTag = TagValidator.NormalizeTag(_tagInput.value ?? "");

            if (!TagValidator.IsValidTag(newTag))
            {
                // UnityEngine.Debug.Log("Tag cannot be empty.");
                return;
            }

            foreach (CommunityTag existingTag in _tagDatabase.communityTags)
            {
                if (existingTag.tagName.ToLower() == newTag.ToLower())
                {
                    existingTag.voteCount++;

                    if (existingTag.voteCount >= 5)
                        existingTag.isOfficial = true;

                    RefreshCommunityTags();
                    _tagInput.value = "";
                    // UnityEngine.Debug.Log("Vote added to: " + existingTag.tagName);
                    return;
                }
            }

            CommunityTag tag = new CommunityTag(newTag);
            _tagDatabase.communityTags.Add(tag);

            RefreshCommunityTags();
            _tagInput.value = "";
            // UnityEngine.Debug.Log("New tag added: " + tag.tagName);
        }

        // === CODE FROM main2/Assets/Scripts/CommunityTagSystem/CommunityTagManager.cs === LINE 174-217 (RefreshCommunityTags / CreateTagUI) ===
        private void RefreshCommunityTags()
        {
            _officialRow.Clear();
            _communityRow.Clear();

            foreach (CommunityTag tag in _tagDatabase.communityTags)
            {
                var chip = new Label(tag.isOfficial ? tag.tagName : $"{tag.tagName} ({tag.voteCount})");
                chip.AddToClassList("tag");
                chip.AddToClassList(tag.isOfficial ? "tag--active" : "tag--community");

                if (tag.isOfficial) _officialRow.Add(chip);
                else _communityRow.Add(chip);
            }
        }
    }
}

// =====END OF ZAHRA SECTION=====
