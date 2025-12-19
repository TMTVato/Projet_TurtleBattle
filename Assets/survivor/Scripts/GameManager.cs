using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerLogic player;

    public static GameManager instance;

    public PoolManager poolManager;

    public float gameTime;
    public float maxGameTime = 2*10f;
    void Awake()
    {
        instance = this;
    }

    
    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }

    }
}
