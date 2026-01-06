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
        // Récupère tous les points de spawn 
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
        // Ajuste les poids de spawn (plus ou moins d'ennemis) en fonction du temps de jeu
        float t = GameManager.instance.gameTime / GameManager.instance.maxGameTime;
        for (int i = 0; i < spawnData.Length; i++)
        {
            spawnData[i].spawnWeight = Mathf.Lerp(1f, 5f, t * i / (spawnData.Length - 1));
        }

        float spawnTime = spawnData[0].spawnTime; // Temps intervalle de spawn de base 
        if (isEventActive) spawnTime /= eventSpawnMultiplier;
        //Si le timer dépasse le temps de spawn, on spawn un ennemi
        if (timer > spawnTime)
        {
            Spawn();
            timer = 0; //reset du timer
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
    // Sélectionne un SpawnData en fonction de son poids et du temps de jeu
    SpawnData GetWeightedRandomSpawnData()
    {
        float currentTime = GameManager.instance.gameTime;
        List<SpawnData> unlocked = new List<SpawnData>();
        float totalWeight = 0f;
        // Récupère les SpawnData débloqués en fonction du temps de jeu
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
        // Sélectionne un SpawnData en fonction de son poids
        float rand = Random.Range(0, totalWeight);
        float sum = 0f;
        // Parcourt les SpawnData débloqués pour trouver celui correspondant au poids aléatoire
        foreach (var data in unlocked)
        {
            sum += data.spawnWeight;
            if (rand <= sum)
                return data;
        }
        return unlocked[0];
    }
    // Spawn un ennemi avec les données sélectionnées
    void Spawn()
    {
        SpawnData data = GetWeightedRandomSpawnData(); // Récupère un SpawnData en fonction du poids
        GameObject enemy = GameManager.instance.poolManager.Get(0); // Récupère un ennemi depuis le PoolManager

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
        logic.Initialisation(data); // Initialise l'ennemi avec les données de spawn
    }
}

[System.Serializable]
public class SpawnData
{
    //Attributs ennemi à spawn
    public int spriteType;
    public float unlockTime = 0f; // Temps de jeu minimal avant apparition de cet ennemi
    public float spawnTime = 0.5f;
    public int health = 10;
    public float speed = 2f;
    public float spawnWeight = 1f; //plus ou moins de chance de spawn
    public GameObject Target; // Cible de l'ennemi
}
