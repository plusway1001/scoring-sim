using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject step1;
    public GameObject step2;
    public GameObject step3;
    public GameObject step4;

    private void Start()
    {
        step1.SetActive(true);
        step2.SetActive(false);
        step3.SetActive(false);
        step4.SetActive(false);
    }

    public void GoToStep2()
    {
        step1.SetActive(false);
        step2.SetActive(true);
    }

    public void GoToStep3()
    {
        step2.SetActive(false);
        step3.SetActive(true);
    }

    public void GoToStep4()
    {
        step3.SetActive(false);
        step4.SetActive(true);
    }

    public void BackToStep1()
    {
        step2.SetActive(false);
        step1.SetActive(true);
    }

    public void BackToStep2()
    {
        step3.SetActive(false);
        step2.SetActive(true);
    }

    public void BackToStep3()
    {
        step4.SetActive(false);
        step3.SetActive(true);
    }
}
