using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour
{
    public static GameController control;

    public static float maxHealthP1 = 100f;
    public static float maxHealthP2 = 100f;

    public static float maxShieldP1 = 100f;
    public static float maxShieldP2 = 100f;

    public static float maxMagP1 = 5;
    public static float maxMagP2 = 5;

    public static float currentShieldP1;
    public static float currentShieldP2;

    public static float totalAmmoUsedP1;
    public static float totalAmmoUsedP2;

    public static string gameControllerTimer;

    private void Awake()
    {
        GameControllerSingleton();
    }

    private void GameControllerSingleton()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    } 
}

