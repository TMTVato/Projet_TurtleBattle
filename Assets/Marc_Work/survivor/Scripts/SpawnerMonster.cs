using System.Collections.Generic;
using UnityEngine;

public class SpawnerMonster : MonoBehaviour
{
    public SpawnData[] spawnData;
    public Transform[] spawnPoints;
    private float timer;
    public float eventSpawnMultiplier = 3f;

    private bool isEventActive = false;
    private float eventTimer = 0f;
    private float eventDuration = 0f;

    private void Awake()
    {
        List<Transform> points = new List<Transform>();
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t != this.transform)
                points.Add(t);
        }
        spawnPoints = points.ToArray();
    }

    void Update()
    {
        if (!GameManager.instance.isLive) return;

        timer += Time.deltaTime;

        float t = GameManager.instance.gameTime / GameManager.instance.maxGameTime;
        for (int i = 0; i < spawnData.Length; i++)
        {
            spawnData[i].spawnWeight = Mathf.Lerp(1f, 5f, t * i / (spawnData.Length - 1));
        }

        float spawnTime = spawnData[0].spawnTime;
        if (isEventActive) spawnTime /= eventSpawnMultiplier;

        if (timer > spawnTime)
        {
            Spawn();
            timer = 0;
        }

        if (isEventActive)
        {
            eventTimer += Time.deltaTime;
            if (eventTimer >= eventDuration)
            {
                isEventActive = false;
            }
        }
    }

    // Appelée par EventManager
    public void StartEvent(float duration)
    {
        isEventActive = true;
        eventTimer = 0f;
        eventDuration = duration;
    }

    // Appelée par EventManager
    public void EndEvent()
    {
        isEventActive = false;
        eventTimer = 0f;
    }

    SpawnData GetWeightedRandomSpawnData()
    {
        float currentTime = GameManager.instance.gameTime;
        List<SpawnData> unlocked = new List<SpawnData>();
        float totalWeight = 0f;

        foreach (var data in spawnData)
        {
            if (currentTime >= data.unlockTime)
            {
                unlocked.Add(data);
                totalWeight += data.spawnWeight;
            }
        }

        if (unlocked.Count == 0)
            return spawnData[0];

        float rand = Random.Range(0, totalWeight);
        float sum = 0f;
        foreach (var data in unlocked)
        {
            sum += data.spawnWeight;
            if (rand <= sum)
                return data;
        }
        return unlocked[0];
    }

    void Spawn()
    {
        SpawnData data = GetWeightedRandomSpawnData();
        GameObject enemy = GameManager.instance.poolManager.Get(0);

        if (enemy == null)
        {
            Debug.LogError("Impossible d'obtenir un ennemi depuis le PoolManager.");
            return;
        }

        EnemyLogic logic = enemy.GetComponent<EnemyLogic>();
        if (logic == null)
        {
            Debug.LogError("Le prefab d'ennemi n'a pas de composant EnemyLogic.");
            return;
        }

        enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        logic.Initialisation(data);
    }
}

[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public float unlockTime = 0f;
    public float spawnTime = 0.5f;
    public int health = 10;
    public float speed = 2f;
    public float spawnWeight = 1f;
    public GameObject Target;
}
