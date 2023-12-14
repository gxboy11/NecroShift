using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public int defeatedEnemies;
    public int vida;
    public int monedas;
    public int balas;
    void Start()
    {
        defeatedEnemies = 0;
        vida = 10;
    }

   
    void Update()
    {
        if (vida < 1 )
        {
            GameOver();
        }
    }

    void GameOver()
    {

    }
}
