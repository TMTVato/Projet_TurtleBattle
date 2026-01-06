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
    private GameObject currentChest;
    private PlayerChestArrow playerArrow;

    void Start()
    {
        spawner = FindObjectOfType<SpawnerMonster>();
        playerArrow = FindObjectOfType<PlayerChestArrow>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive) return;

        if (!eventActive)
        {
            // Gère le timer pour déclencher un nouvel événement
            eventTimer += Time.deltaTime;
            if (eventTimer >= eventInterval)
            {
                eventTimer = 0f;
                SpawnChestEvent();
            }
        }
    }
    // Déclenchement de l'événement coffre
    public void SpawnChestEvent()
    {
        eventActive = true;
        // Spawn du coffre à une position aléatoire
        Vector3 chestPos = GetRandomPositionOnMap();
        //Instantiate du coffre
        currentChest = Instantiate(chestPrefab, chestPos, Quaternion.identity);

        float radius = 3f;

        UIManager.Instance.ShowBanner("Un événement est apparu !");

        if (spawner != null)
            spawner.StartEvent(eventDuration);
        // Indique la position du coffre à l'UI flèche
        if (playerArrow != null)
            playerArrow.SetChestTarget(currentChest.transform);
    }

    // Fin de l'événement coffre
    public void EndChestEvent(bool victory)
    {
        eventActive = false;

        if (playerArrow != null)
            playerArrow.ClearChestTarget();

        if (currentChest != null)
            Destroy(currentChest);
        currentChest = null;

        if (spawner != null)
            spawner.EndEvent();

        if (victory)
        {
            if (turretUIManager != null)
                turretUIManager.UnlockNextOrIncreaseLimit(); // Débloque la tourelle suivante ou augmente la limite
            UIManager.Instance.ShowBanner("Victoire de l'événement !");
            AudioManager.instance.PlaySFX(AudioManager.SFX.LevelUp);
        }
        else
        {
            UIManager.Instance.ShowBanner("Échec de l'événement !");
        }
    }
    // Génère une position aléatoire dans la zone définie
    private Vector3 GetRandomPositionOnMap()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector3(x, y, 0);
    }
}