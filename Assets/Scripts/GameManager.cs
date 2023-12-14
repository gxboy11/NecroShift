using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : Singleton<GameManager>
{
    public int defeatedEnemies;
    public int vidaPlayer;
    public int monedas;

    void Start()
    {
        defeatedEnemies = 0;
        monedas = 0;
    }

    public void GameOver()
    {
        //Escene GameOver
    }

    public void SetDefeatedEnemy()
    {
        defeatedEnemies++;
    }

    public void SetPlayerDamage(int receivedDamage)
    {
        vidaPlayer -= receivedDamage;
    }

    public void IncrementCoins(int receivedCoins)
    {
        monedas += receivedCoins;
    }

    public int GetCoins()
    {
        return monedas;
    }

    public int GetDefeatedEnemies()
    {
        return defeatedEnemies;
    }
}
