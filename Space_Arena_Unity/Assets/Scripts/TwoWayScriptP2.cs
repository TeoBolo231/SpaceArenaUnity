using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayScriptP2 : MonoBehaviour
{
    private PlatformEffector2D myEffetor;
    public Player2Script player2;

    void Start()
    {
        myEffetor = GetComponent<PlatformEffector2D>();
    }


    void Update()
    {
        if (player2.moveAxisP2.y < -0.85) //allows player 2 to cross platforms downwards
        {
            myEffetor.rotationalOffset = 180;
        }

        if (player2.moveAxisP2.y >= -0.85) //allows player 2 to cross platforms upwards
        {
            myEffetor.rotationalOffset = 0;
        }
    }
}
