using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    

    public PlayerLogic player;
    public TurtleLogic turtle;
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
    public int[] nextExp = {
    5, 10, 16, 23, 31, 40, 50, 61, 73, 86,
    100, 115, 131, 148, 166, 185, 205, 226, 248, 271,
    295, 320, 346, 373, 401
};

    [Header("#Turtle Info")]
    public float turtleHP;
    public float turtleMaxHP = 100;
    public float turtleregen;

    [Header("#Bonus Stats Turrets")]
    public float bonusDamage = 0f;
    public float bonusFireRate = 0f;
    public float bonusPenetration = 0f;
    public float bonusSpeed = 0f;
    public float bonusRange = 0f;

    void Awake()
    {
        instance = this;                                                                                                                                                                                                                                                                                                            
    }
    private void Start()
    {
        HP = maxHP;
        turtleHP = turtleMaxHP;
        uiLevelUp.Select(0);
        Resume();
        AudioManager.instance.PlayBGM(true);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }
    IEnumerator GameOverRoutine()
    {
        //gère le game over en affichant l'écran de fin et en stoppant le jeu
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        //stop the game
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
        AudioManager.instance.PlayBGM(false);
        AudioManager.instance.PlaySFX(AudioManager.SFX.Lose);
    }


    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }
    IEnumerator GameVictoryRoutine()
    {
        //gère la victoire en affichant l'écran de victoire et en stoppant le jeu
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //stop the game
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
        AudioManager.instance.PlayBGM(false);
        AudioManager.instance.PlaySFX(AudioManager.SFX.Win);
    }

    void Update()
    {
        //Regen la vie du turtle
        //turtleHP += turtleregen * Time.deltaTime;


        if (!isLive) return;
        gameTime += Time.deltaTime;
        //Vérifie si le temps de jeu a atteint le maximum = fin du jeu
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }

    }
    // Gagne de l'expérience
    public void GetExp()
    {
        if (!isLive) return;
        exp++;
        // Vérifie si le joueur a atteint l'expérience nécessaire pour monter de niveau
        if (exp == nextExp[Mathf.Min(level, nextExp.Length -1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    // Stop le jeu
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; // Pause le jeu
    }

    // Resume le jeu
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1; // Resume valeur normale du temps
    }

    public void Reset() //Reload la scène actuelle
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


