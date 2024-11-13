using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public AudioClip[] gameSoundtracks; //array containing all the soundtracks need to the game
    
    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadPlayAgain()
    {
        SceneManager.LoadScene(1);

        AudioManager.instance.PlayMusic(gameSoundtracks[0]);
    }

    public void Level1()
    {
        Player1Script.isAliveP1 = true; //reset players' status and ammo used every time the level scene is loaded
        Player2Script.isAliveP2 = true;

        Player1Script.totalAmmoUsedP1 = 0f;
        Player2Script.totalAmmoUsedP2 = 0f;

        SceneManager.LoadScene(2);

        AudioManager.instance.PlayMusic(gameSoundtracks[1]);
    }

    public void LoadControlScreen1()
    {
        SceneManager.LoadScene(5);
    }

    public void LoadControlScreen2()
    {
        SceneManager.LoadScene(6);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
