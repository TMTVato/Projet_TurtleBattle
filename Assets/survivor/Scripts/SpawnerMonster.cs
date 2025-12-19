using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMonster : MonoBehaviour
{
    public SpawnData[] spawnData;
    public Transform[] spawnPoints;
    int level;
    private float timer;
    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }



    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f),spawnData.Length - 1);
        if (timer > spawnData[level].spawnTime)
        {
            Spawn();
            timer = 0;
        }

    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.poolManager.Get(0);
        enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        enemy.GetComponent<EnemyLogic>().Initialisation(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public float spawnTime;

    public int health;
    public float speed;
}