using UnityEngine;
using UnityEngine.UI;

public class ChestEventZone : MonoBehaviour
{
    public float eventDuration = 30f;
    private float timer = 0f;
    private bool eventActive = true;
    public float timeRemaining;
    public EventManager eventManager;
    public SpriteRenderer zoneRenderer;
    public Color zoneColor = new Color(0.2f, 0.8f, 0.2f, 0.5f);

    private bool playerInZone = false;
    private PlayerChestArrow playerArrow;

    [Header("Unlock Settings")]
    public float unlockDuration = 10f;
    private float unlockTimer = 0f;
    private bool chestUnlocked = false;

    [Header("UI Progress")]
    private Slider progressSlider;
    private GameObject sliderCanvas;
    private CanvasGroup canvasGroup;

    void Start()
    {
        timer = 0f;
        unlockTimer = 0f;
        if (eventManager == null)
            eventManager = FindObjectOfType<EventManager>();
        if (zoneRenderer != null)
            zoneRenderer.color = zoneColor;
        timeRemaining = eventDuration;
        playerArrow = FindObjectOfType<PlayerChestArrow>();

        FindProgressUI(); // Trouver et configurer le Slider de progression

        if (progressSlider != null)
        {
            progressSlider.minValue = 0f;
            progressSlider.maxValue = unlockDuration;
            progressSlider.value = 0f;
        }

        if (sliderCanvas != null)
        {
            canvasGroup = sliderCanvas.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = sliderCanvas.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            Debug.Log("Timer Slider initialisé et masqué");
        }
    }
    // Méthode pour trouver le Slider dans la scène
    void FindProgressUI()
    {
        GameObject timerSlider = GameObject.FindGameObjectWithTag("ChestTimer");

        if (timerSlider != null)
        {
            sliderCanvas = timerSlider;
            progressSlider = timerSlider.GetComponent<Slider>();

            if (progressSlider != null)
            {
                Debug.Log("Timer Slider avec tag 'ChestTimer' trouvé et configuré !");
            }
            else
            {
                Debug.LogWarning("GameObject avec tag 'ChestTimer' trouvé mais aucun composant Slider dessus.");
            }
        }
        else
        {
            Debug.LogWarning("Aucun GameObject avec le tag 'ChestTimer' trouvé dans la scène.");
        }
    }

    void Update()
    {
        if (!eventActive) return;

        timer += Time.deltaTime;
        timeRemaining = Mathf.Max(0f, eventDuration - timer);

        if (playerInZone && !chestUnlocked) //Si le joueur est dans la zone et le coffre n'est pas déverrouillé
        {
            unlockTimer += Time.deltaTime; // Augmente le timer de déverrouillage

            if (progressSlider != null)
                progressSlider.value = unlockTimer;

            if (unlockTimer >= unlockDuration) // Coffre déverrouillé
            {
                UnlockChest();
                EndEventImmediately(true); // Terminer immédiatement avec succès
                return;
            }
        }

        if (timer >= eventDuration)
        {
            EndEventImmediately(chestUnlocked); // Terminer avec échec si pas déverrouillé
        }
    }

    void UnlockChest() //Déverrouille le coffre 
    {
        chestUnlocked = true;
        Debug.Log("Coffre déverrouillé !");
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            Debug.Log("Timer Slider masqué après déverrouillage");
        }
    }

    void EndEventImmediately(bool success) // Termine l'événement immédiatement si succès
    {
        eventActive = false;
        Debug.Log("Fin immédiate de l'événement. Succès : " + success);
        if (eventManager != null)
        {
            eventManager.EndChestEvent(success);
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) // Détecte l'entrée du joueur dans la zone
    {
        // Vérifie si c'est le joueur qui entre dans la zone
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInZone = true;
            Debug.Log("Joueur est entré dans la zone.");
            if (playerArrow != null)
                playerArrow.SetInZone(true);
            if (canvasGroup != null && !chestUnlocked)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                Debug.Log("Timer Slider ACTIVÉ - Alpha: " + canvasGroup.alpha);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) // Détecte la sortie du joueur de la zone
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Réinitialise le timer de déverrouillage
            playerInZone = false;
            unlockTimer = 0f;
            Debug.Log("Joueur a quitté la zone. Progression réinitialisée.");


            if (playerArrow != null)
                playerArrow.SetInZone(false);

            if (progressSlider != null)
                progressSlider.value = 0f;

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                Debug.Log("Timer Slider masqué");
            }
        }
    }
}