using UnityEngine;
public class ChestEventZone : MonoBehaviour
{
    public float eventDuration = 5;
    private float timer = 0f;
    private bool eventActive = true;
    public float timeRemaining;
    public EventManager eventManager;
    public SpriteRenderer zoneRenderer;
    public Color zoneColor = new Color(0.2f, 0.8f, 0.2f, 0.5f);

    private bool playerInZone = false;
    private PlayerChestArrow playerArrow;

    void Start()
    {
        timer = 0f;
        if (eventManager == null)
            eventManager = FindObjectOfType<EventManager>();
        if (zoneRenderer != null)
            zoneRenderer.color = zoneColor;
        timeRemaining = eventDuration;
        playerArrow = FindObjectOfType<PlayerChestArrow>();
    }

    void Update()
    {
        if (!eventActive) return;

        timer += Time.deltaTime;
        timeRemaining = Mathf.Max(0f, eventDuration - timer);

        if (timer >= eventDuration)
        {
            eventActive = false;
            Debug.Log("Fin de l'événement. Joueur dans la zone : " + playerInZone);
            eventManager.EndChestEvent(playerInZone);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInZone = true;
            Debug.Log("Joueur est entré dans la zone.");
            if (playerArrow != null)
                playerArrow.SetInZone(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInZone = false;
            Debug.Log("Joueur a quitté la zone.");
            if (playerArrow != null)
                playerArrow.SetInZone(false);
        }
    }
}