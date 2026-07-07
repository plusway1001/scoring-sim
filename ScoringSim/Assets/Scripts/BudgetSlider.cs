using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BudgetSlider : MonoBehaviour
{
    public Slider slider;
    public TMP_Text budgetValueText;

    void Start()
    {
        slider.onValueChanged.AddListener(UpdateBudget);

        UpdateBudget(slider.value);
    }

    void UpdateBudget(float value)
    {
        UserProfileManager.Instance.currentUser.preferences.maxPrice = value;

        budgetValueText.text = "Max Budget: $" + value.ToString("0");
    }
}