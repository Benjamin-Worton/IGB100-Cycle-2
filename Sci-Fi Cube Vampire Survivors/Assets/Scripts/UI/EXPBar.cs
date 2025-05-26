using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
{

    public Slider slider;

    public void SetMaxEXP(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetEXP(float health)
    {
        slider.value = health;
    }
}
