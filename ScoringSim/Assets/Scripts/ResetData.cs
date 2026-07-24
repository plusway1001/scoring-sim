using System.IO;
using UnityEngine;

public class ResetData : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClearUserDatabase();
    }

    private void ClearUserDatabase()
    {
        string path = Path.Combine(Application.dataPath, "JSON/user.json");

        UserDatabase database = new UserDatabase();
        database.users.Clear();

        string json = JsonUtility.ToJson(database, true);
        File.WriteAllText(path, json);

        Debug.Log("User database cleared.");
    }
}
