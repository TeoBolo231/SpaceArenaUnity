using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderManager : MonoBehaviour
{
    [SerializeField] public float gamehHealth = 20f;
    [SerializeField] public float gameShield = 20f;
    [SerializeField] public float gameMag = 10f;

    public Slider sliderHealthP1;
    public Slider sliderShieldP1;
    public Slider sliderMaxMagP1;

    public Slider sliderHealthP2;
    public Slider sliderShieldP2;
    public Slider sliderMaxMagP2;

    private float hpP1;
    private float ShP1;

    private float hpP2;
    private float ShP2;

    private float amP1;
    private float amP2;

    [SerializeField] public TextMeshProUGUI textHealthP1;
    [SerializeField] public TextMeshProUGUI textHealthP2;

    [SerializeField] public TextMeshProUGUI textShieldP1;
    [SerializeField] public TextMeshProUGUI textShieldP2;

    [SerializeField] public TextMeshProUGUI textAmmoP1;
    [SerializeField] public TextMeshProUGUI textAmmoP2;

   

    private void Awake()
    {
        sliderHealthP1.maxValue = gamehHealth;
        sliderShieldP1.maxValue = gameShield;
        sliderMaxMagP1.maxValue = gameMag;

        sliderHealthP2.maxValue = gamehHealth;
        sliderShieldP2.maxValue = gameShield;
        sliderMaxMagP2.maxValue = gameMag;

        SetMatchStatsP1(GameController.maxHealthP1 / 10, GameController.maxShieldP1 / 10, GameController.maxMagP1);
        SetMatchStatsP2(GameController.maxHealthP2 / 10, GameController.maxShieldP2 / 10, GameController.maxMagP2);
        
    }
    void Update()
    {
        UpdateUiText();
        ValueMulti();
    }

    public void SetMatchStatsP1(float health, float shield, float ammo)
    {
        sliderHealthP1.value = health;
        sliderShieldP1.value = shield;
        sliderMaxMagP1.value = ammo;
    }

    public void SetMatchStatsP2(float health, float shield, float ammo)
    {
        sliderHealthP2.value = health;
        sliderShieldP2.value = shield;
        sliderMaxMagP2.value = ammo;
    }

    private void ValueMulti()
    {
        hpP1 = sliderHealthP1.value * 10;
        ShP1 = sliderShieldP1.value * 10;

        hpP2 = sliderHealthP2.value * 10;
        ShP2 = sliderShieldP2.value * 10;

        amP1 = sliderMaxMagP1.value;
        amP2 = sliderMaxMagP2.value;
    }

    private void UpdateUiText()
    {
        textHealthP1.text = hpP1.ToString();
        textShieldP1.text = ShP1.ToString();
        textAmmoP1.text = amP1.ToString();

        textHealthP2.text = hpP2.ToString();
        textShieldP2.text = ShP2.ToString();
        textAmmoP2.text = amP2.ToString();
    }

    private void OnDestroy()
    {
        GameController.maxHealthP1 = hpP1;
        GameController.maxHealthP2 = hpP2;
        GameController.maxShieldP1 = ShP1;
        GameController.maxShieldP2 = ShP2;
        GameController.maxMagP1 = amP1;
        GameController.maxMagP2 = amP2;
    }
}
