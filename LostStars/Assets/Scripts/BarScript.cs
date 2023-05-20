using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    public Slider Slider;

    public void Set(float current, float max)
    {
        Slider.value = current;
        Slider.maxValue = max;
    }
}
