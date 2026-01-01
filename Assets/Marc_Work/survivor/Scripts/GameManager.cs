using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    

    public PlayerLogic player;
    [Header("#GameObject")]
    public static GameManager instance;
    public PoolManager poolManager;
    public LevelUp uiLevelUp;
    public EndGame uiResult;
    public GameObject enemyCleaner;

    [Header("#Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2*10f;
    [Header("#player Info")]
    public int level;
    public int kill;
    public int exp;
    public float HP;
    public float maxHP = 100;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };


    void Awake()
    {
        instance = this;                                                                                                                                                                                                                                                                                                            
    }
    private void Start()
    {
        HP = maxHP;
        uiLevelUp.Select(0);
        Resume();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }
    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        //stop the game
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
    }


    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }
    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //stop the game
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
    }

    void Update()
    {
        if (!isLive) return;
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }

    }

    public void GetExp()
    {
        if (!isLive) return;
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

    public void Reset() // Restart the current scene when called
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


