using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayScriptP1 : MonoBehaviour
{
    private PlatformEffector2D myEffetor;
    public Player1Script player1;

    void Start()
    {
        myEffetor = GetComponent<PlatformEffector2D>(); 
    }

    
    void Update()
    {
        if (player1.moveAxisP1.y < -0.85) //allows player 1 to cross platforms downwards
        {
            myEffetor.rotationalOffset = 180;
        }

        if (player1.moveAxisP1.y >= -0.85) //allows player 1 to cross platforms upwards
        {
            myEffetor.rotationalOffset = 0;
        }

    }
}
