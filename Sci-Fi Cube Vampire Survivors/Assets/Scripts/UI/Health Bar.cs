using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{

    public Slider slider;

    public void SetMax(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetCurrent(float health)
    {
        slider.value = health;
    }
}
