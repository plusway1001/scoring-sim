using UnityEngine;

public class ProfileSettingsPopup : MonoBehaviour
{
    public GameObject profileSettingsPanel;

    public void OpenPopup()
    {
        profileSettingsPanel.SetActive(true);
    }

    public void ClosePopup()
    {
        profileSettingsPanel.SetActive(false);
    }
}
