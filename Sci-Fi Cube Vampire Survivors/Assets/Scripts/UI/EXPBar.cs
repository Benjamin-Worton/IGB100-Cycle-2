using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxEXP(int exp)
    {
        slider.maxValue = exp;
        slider.value = 0;
    }

    public void SetEXP(int exp)
    {
        slider.value = exp;
    }
}