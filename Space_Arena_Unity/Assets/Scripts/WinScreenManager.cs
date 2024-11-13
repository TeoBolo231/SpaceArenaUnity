using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinScreenManager : MonoBehaviour
{ 
    [SerializeField] public TextMeshProUGUI shieldP1UItext;
    [SerializeField] public TextMeshProUGUI totalAmmoP1UItext;
    [SerializeField] public TextMeshProUGUI shieldP2UItext;
    [SerializeField] public TextMeshProUGUI totalAmmoP2UItext;
    [SerializeField] public TextMeshProUGUI timerUitext;

    void Start()
    {
        UpdateUItext();
    }

    public void UpdateUItext()
    {

        shieldP1UItext.text = GameController.currentShieldP1.ToString();
        totalAmmoP1UItext.text = GameController.totalAmmoUsedP1.ToString();

        shieldP2UItext.text = GameController.currentShieldP2.ToString();
        totalAmmoP2UItext.text = GameController.totalAmmoUsedP2.ToString();

        timerUitext.text = GameController.gameControllerTimer;
    }
}
