using TMPro;
using UnityEngine;

public class CommunityTagManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField tagInput;

    public Transform officialTagContainer;
    public Transform communityTagContainer;

    public GameObject communityTagPrefab;

    [Header("Database")]
    public TagDatabase tagDatabase = new TagDatabase();

    private void Start()
    {
        // Official game tags
        tagDatabase.communityTags.Add(new CommunityTag("Platformer", true));
        tagDatabase.communityTags.Add(new CommunityTag("2D", true));
        tagDatabase.communityTags.Add(new CommunityTag("Metroidvania", true));

        // Initial community tags
        tagDatabase.communityTags.Add(new CommunityTag("Souls-like", 4));
        tagDatabase.communityTags.Add(new CommunityTag("Cozy", 1));

        RefreshCommunityTags();
    }

    public void SubmitTag()
    {
        string newTag = TagValidator.NormalizeTag(tagInput.text);

        // Check if input is empty
        if (!TagValidator.IsValidTag(newTag))
        {
            Debug.Log("Tag cannot be empty.");
            return;
        }

        // Check if tag already exists
        foreach (CommunityTag existingTag in tagDatabase.communityTags)
        {
            if (existingTag.tagName.ToLower() == newTag.ToLower())
            {
                existingTag.voteCount++;

                // Promote to official after 5 votes
                if (existingTag.voteCount >= 5)
                {
                    existingTag.isOfficial = true;
                }

                RefreshCommunityTags();

                tagInput.text = "";

                Debug.Log("Vote added to: " + existingTag.tagName);

                return;
            }
        }

        // Create a brand new community tag
        CommunityTag tag = new CommunityTag(newTag);

        tagDatabase.communityTags.Add(tag);

        RefreshCommunityTags();

        tagInput.text = "";

        Debug.Log("New tag added: " + tag.tagName);
    }

    private void RefreshCommunityTags()
    {
        // Clear Official Tag UI
        foreach (Transform child in officialTagContainer)
        {
            Destroy(child.gameObject);
        }

        // Clear Community Tag UI
        foreach (Transform child in communityTagContainer)
        {
            Destroy(child.gameObject);
        }

        // Rebuild UI
        foreach (CommunityTag tag in tagDatabase.communityTags)
        {
            if (tag.isOfficial)
            {
                CreateTagUI(tag, officialTagContainer);
            }
            else
            {
                CreateTagUI(tag, communityTagContainer);
            }
        }
    }

    private void CreateTagUI(CommunityTag tag, Transform parent)
    {
        GameObject tagButton = Instantiate(communityTagPrefab, parent);

        TMP_Text text = tagButton.GetComponentInChildren<TMP_Text>();

        // Official tags only show the name
        if (tag.isOfficial)
        {
            text.text = tag.tagName;
        }
        else
        {
            text.text = $"{tag.tagName} ({tag.voteCount})";
        }
    }
}