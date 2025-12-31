using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    

    public PlayerLogic player;
    [Header("#GameObject")]
    public static GameManager instance;
    public PoolManager poolManager;
    public LevelUp uiLevelUp;

    [Header("#Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2*10f;
    [Header("#player Info")]
    public int level;
    public int kill;
    public int exp;
    public int HP;
    public int maxHP = 100;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };


    void Awake()
    {
        instance = this;                                                                                                                                                                                                                                                                                                            
    }
    private void Start()
    {
        HP = maxHP;
        uiLevelUp.Select(0);
    }

    void Update()
    {
        if (!isLive) return;
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }

    }

    public void GetExp()
    {
        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length -1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    // Stop the game
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; // Pause the game
    }

    // Resume the game
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1; // Resume normal time scale
    }
}

