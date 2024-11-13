using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBarScript : MonoBehaviour
{
    public Slider slider;

    public void SetShield(float shield)
    {
        slider.value = shield;
    }

    public void SetMaxShield(float shield)
    {
        slider.maxValue = shield;
        slider.value = shield;
    }
}
