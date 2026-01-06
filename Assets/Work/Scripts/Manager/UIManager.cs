using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject bannerPanel; // Le panneau du bandeau (UI)
    public Text bannerText;        // Le texte du bandeau
    public float bannerDuration = 2.5f;

    private Coroutine bannerCoroutine;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    // Affiche un bandeau avec un message 
    public void ShowBanner(string message)
    {
        if (bannerCoroutine != null)
            StopCoroutine(bannerCoroutine);

        bannerCoroutine = StartCoroutine(ShowBannerCoroutine(message));
    }
    // Coroutine pour afficher le bandeau pendant une durée définie
    private IEnumerator ShowBannerCoroutine(string message)
    {
        bannerPanel.SetActive(true);
        bannerText.text = message;
        yield return new WaitForSeconds(bannerDuration);
        bannerPanel.SetActive(false);
    }
}