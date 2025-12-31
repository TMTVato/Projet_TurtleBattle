using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    

    public PlayerLogic player;
    [Header("#GameObject")]
    public static GameManager instance;

    public PoolManager poolManager;

    [Header("#Game Control")]
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
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }

    }

    public void GetExp()
    {
        exp++;

        if (exp == nextExp[level])
        {
            level++;
            exp = 0;
        }
    }
}

