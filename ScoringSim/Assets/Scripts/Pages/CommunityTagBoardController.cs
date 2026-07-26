// === ADAPTED FROM main2/Assets/Scripts/CommunityTagSystem/CommunityTagManager.cs === LINE 1-218 ===
// The vote-counting / promotion-to-official logic (SubmitTag) is preserved exactly
// as main2 wrote it — same "5 votes promotes a tag to official" rule, same
// duplicate-vote-increments-count behaviour, same TagValidator/CommunityTag/TagDatabase
// classes (copied verbatim in this folder). What had to change is the UI plumbing:
// main2's version used TMP_InputField + Transform containers + Instantiate(prefab)
// for UGUI; this version queries a UI Toolkit TextField/Button/VisualElement set
// (see the "Community Tags" block added to GameDetailPage.uxml) and builds Label
// chips instead of instantiating a prefab.
//
// main2's CommunityTagManager also only ever tracked ONE global TagDatabase (it was
// a MonoBehaviour on a single scene object). GameScope has a catalogue of many
// games, so this adaptation keeps one TagDatabase per game (keyed by GameEntry.Id)
// and seeds each with that game's own Tags as the starting "official" tags, instead
// of main2's three hardcoded names ("Platformer", "2D", "Metroidvania").
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameScope.Pages
{
    public class CommunityTagBoardController
    {
        // Session-only per-game tag boards (SDD Priority 5 is a stretch goal / simulation
        // feature — this is deliberately not persisted to disk, same as the rest of GameScope's
        // in-memory catalogue state).
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
                // main2 seeded 3 hardcoded official tags + 2 starter community tags; here we
                // seed the official tags from the game's own catalogue Tags instead, since
                // every game has different tags (see class header for why).
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
        // Vote/duplicate/promotion logic unchanged; only the UI calls at the edges (reading
        // the TextField's value instead of TMP_InputField.text, clearing it the same way) differ.
        public void SubmitTag()
        {
            string newTag = TagValidator.NormalizeTag(_tagInput.value ?? "");

            if (!TagValidator.IsValidTag(newTag))
            {
                UnityEngine.Debug.Log("Tag cannot be empty.");
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
                    UnityEngine.Debug.Log("Vote added to: " + existingTag.tagName);
                    return;
                }
            }

            CommunityTag tag = new CommunityTag(newTag);
            _tagDatabase.communityTags.Add(tag);

            RefreshCommunityTags();
            _tagInput.value = "";
            UnityEngine.Debug.Log("New tag added: " + tag.tagName);
        }

        // === CODE FROM main2/Assets/Scripts/CommunityTagSystem/CommunityTagManager.cs === LINE 174-217 (RefreshCommunityTags / CreateTagUI) ===
        // Same official-vs-community split; rebuilt with UI Toolkit Labels instead of
        // Instantiate(communityTagPrefab, parent) + GetComponentInChildren<TMP_Text>().
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
