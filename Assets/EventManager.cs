using UnityEngine;

public class EventManager : MonoBehaviour
{
    public GameObject chestPrefab;
    public GameObject[] turretPrefabs;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    public float eventDuration = 20f;
    public float eventInterval = 20f;
    public TurretUIManager turretUIManager;

    private float eventTimer = 0f;
    private bool eventActive = false;
    private SpawnerMonster spawner;

    void Start()
    {
        spawner = FindObjectOfType<SpawnerMonster>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive) return;

        if (!eventActive)
        {
            eventTimer += Time.deltaTime;
            if (eventTimer >= eventInterval)
            {
                eventTimer = 0f;
                SpawnChestEvent();
            }
        }
    }

    public void SpawnChestEvent()
    {
        eventActive = true;
        Vector3 chestPos = GetRandomPositionOnMap();
        GameObject chest = Instantiate(chestPrefab, chestPos, Quaternion.identity);

        float radius = 3f;
        for (int i = 0; i < 3; i++)
        {
            float angle = i * 120 * Mathf.Deg2Rad;
            Vector3 turretPos = chestPos + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            int turretType = i % turretPrefabs.Length;
            Instantiate(turretPrefabs[turretType], turretPos, Quaternion.identity);
        }

        UIManager.Instance.ShowBanner("Un événement est apparu !");

        if (spawner != null)
            spawner.StartEvent(eventDuration);
    }

    public void EndChestEvent(bool victory)
    {
        eventActive = false;
        if (spawner != null)
            spawner.EndEvent();

        if (victory)
        {
            if (turretUIManager != null)
                turretUIManager.UnlockNextOrIncreaseLimit();
            UIManager.Instance.ShowBanner("Victoire de l'événement !");
        }
        else
        {
            UIManager.Instance.ShowBanner("Échec de l'événement !");
        }
    }

    private Vector3 GetRandomPositionOnMap()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector3(x, y, 0);
    }
}
