using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagBarScript : MonoBehaviour
{
    public Slider slider;

    public void SetMag(float ammo)
    {
        slider.value = ammo;
    }

    public void SetMaxMag(float ammo)
    {
        slider.maxValue = ammo;
        slider.value = ammo;
    }
}
