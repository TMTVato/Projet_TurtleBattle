using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMonster : MonoBehaviour
{
    public SpawnData[] spawnData;
    public Transform[] spawnPoints;
    int level;
    private float timer;
    public float levelTime;

    private void Awake()
    {
        // Récupère tous les points de spawn enfants (sauf le parent)
        List<Transform> points = new List<Transform>();
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t != this.transform)
                points.Add(t);
        }
        spawnPoints = points.ToArray();

        // Calcule la durée d'un palier de niveau
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    void Update()
    {
        if (!GameManager.instance.isLive) return;

        timer += Time.deltaTime;

        // Exemple : augmente le poids des ennemis forts avec le temps
        float t = GameManager.instance.gameTime / GameManager.instance.maxGameTime;
        for (int i = 0; i < spawnData.Length; i++)
        {
            // Suppose que les ennemis forts sont à la fin du tableau
            spawnData[i].spawnWeight = Mathf.Lerp(1f, 5f, t * i / (spawnData.Length - 1));
        }

        if (timer > spawnData[0].spawnTime)
        {
            Spawn();
            timer = 0;
        }
    }
    SpawnData GetWeightedRandomSpawnData()
    {
        float currentTime = GameManager.instance.gameTime;
        List<SpawnData> unlocked = new List<SpawnData>();
        float totalWeight = 0f;

        // Filtre les types débloqués
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

        // Toujours utiliser le prefab à l'index 0
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
    public float unlockTime = 0f; // Temps (en secondes) avant que ce type puisse apparaître
    public float spawnTime = 0.5f; // Valeur par défaut, à ajuster dans l'inspecteur
    public int health = 10;
    public float speed = 2f;
    public float spawnWeight = 1f; // Poids de spawn (modifiable dans l’inspecteur)
}
