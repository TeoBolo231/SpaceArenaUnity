using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathScreenTransition : MonoBehaviour
{
    public AudioClip winMusic;

    private void Update()
    {
        P1WinScreen();
        P2WinScreen();
    }

    private void P1WinScreen() 
    {
        if (Player2Script.isAliveP2 == false)
        {
            SceneManager.LoadScene(3);

            AudioManager.instance.PlayMusic(winMusic);
        }
    } //displays player 1's win screen if player 2 is dead

    private void P2WinScreen() 
    {
        if (Player1Script.isAliveP1 == false)
        {
            SceneManager.LoadScene(4);

            AudioManager.instance.PlayMusic(winMusic);
        }
    } //displays player 2's win screen if player 1 is dead
}
