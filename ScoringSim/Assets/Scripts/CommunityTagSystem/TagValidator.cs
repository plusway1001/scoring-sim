// === CODE FROM main2/Assets/Scripts/CommunityTagSystem/TagValidator.cs === LINE 1-31 ===
// Verbatim from the team's main2 branch — no logic changes.
using System.Collections.Generic;

public static class TagValidator
{
    // Returns false if the user entered nothing
    public static bool IsValidTag(string tagName)
    {
        return !string.IsNullOrWhiteSpace(tagName);
    }

    // Returns true if the tag already exists
    public static bool IsDuplicate(string tagName, List<CommunityTag> communityTags)
    {
        foreach (CommunityTag tag in communityTags)
        {
            if (tag.tagName.ToLower() == tagName.Trim().ToLower())
            {
                return true;
            }
        }

        return false;
    }

    // Cleans the tag before storing it
    public static string NormalizeTag(string tagName)
    {
        return tagName.Trim();
    }
}
