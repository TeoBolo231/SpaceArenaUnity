using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManagerScript : MonoBehaviour
{
    [Header("UI ReferencesP1")]
    [SerializeField] public TextMeshProUGUI healthUiTextP1;
    [SerializeField] public TextMeshProUGUI shieldUiTextP1;
    [SerializeField] public TextMeshProUGUI currentMagUiTextP1;
    [SerializeField] public TextMeshProUGUI maxMagUiTextP1;

    [Header("UI ReferencesP2")]
    [SerializeField] public TextMeshProUGUI healthUiTextP2;
    [SerializeField] public TextMeshProUGUI shieldUiTextP2;
    [SerializeField] public TextMeshProUGUI currentMagUiTextP2;
    [SerializeField] public TextMeshProUGUI maxMagUiTextP2;
  
    void Update()
    {
        UpdateUItext();
    }

    public void UpdateUItext()
    {
        healthUiTextP1.text = Player1Script.currentHealthP1.ToString();
        shieldUiTextP1.text = Player1Script.currentShieldP1.ToString();
        currentMagUiTextP1.text = Player1Script.currentMagP1.ToString();
        maxMagUiTextP1.text = GameController.maxMagP1.ToString();
 
        healthUiTextP2.text = Player2Script.currentHealthP2.ToString();
        shieldUiTextP2.text = Player2Script.currentShieldP2.ToString();
        currentMagUiTextP2.text = Player2Script.currentMagP2.ToString();
        maxMagUiTextP2.text = GameController.maxMagP2.ToString();
    }
}
