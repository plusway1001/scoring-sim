using System;

[Serializable]
public class CommunityTag
{
    public string tagName;
    public int voteCount;
    public bool isOfficial;

    // Default constructor (new community tag starts with 1 vote)
    public CommunityTag(string name)
    {
        tagName = name;
        voteCount = 1;
        isOfficial = false;
    }

    // Constructor for existing tags
    public CommunityTag(string name, int votes)
    {
        tagName = name;
        voteCount = votes;
        isOfficial = votes >= 5;
    }

    // Constructor for official tags
    public CommunityTag(string name, bool official)
    {
        tagName = name;
        voteCount = 0;
        isOfficial = official;
    }
}
