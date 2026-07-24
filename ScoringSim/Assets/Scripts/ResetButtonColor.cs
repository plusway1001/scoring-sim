using UnityEngine;
using UnityEngine.UI;

public class ResetButtonColor : MonoBehaviour
{
    public Button[] button;
    public Color normalColor = Color.white;

    // Update is called once per frame
    public void ResetButtonColour()
    {
        for(int i = 0; i < button.Length; i++)
        {
            button[i].GetComponent<Image>().color = normalColor;
        }
    }
}
