using System.Collections.Generic;
using UnityEngine;

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
