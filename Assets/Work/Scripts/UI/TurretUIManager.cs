using UnityEngine;
using UnityEngine.UI;

public class TurretUIManager : MonoBehaviour
{
    public GameObject[] turretPrefabs;
    public Button[] turretButtons;
    public Text[] turretCountTexts;
    public bool[] unlockedTurrets;
    public int[] maxTurretsPerType;
    private int[] placedTurretsPerType;
    private GameObject selectedTurret;
    private int selectedIndex = -1;

    void Start()
    {
        unlockedTurrets = new bool[turretPrefabs.Length];
        unlockedTurrets[0] = true;
        for (int i = 1; i < unlockedTurrets.Length; i++)
            unlockedTurrets[i] = false;

        placedTurretsPerType = new int[turretPrefabs.Length];
        for (int i = 0; i < placedTurretsPerType.Length; i++)
            placedTurretsPerType[i] = 0;

        UpdateTurretButtons();
    }

    public void SelectTurret(int index)
    {
        //Vérifie si la tourelle est débloquée avant de la sélectionner
        if (index >= 0 && index < turretPrefabs.Length && unlockedTurrets[index])
        {
            selectedTurret = turretPrefabs[index];
            selectedIndex = index;
            AudioManager.instance.PlaySFX(AudioManager.SFX.Select);
        }
    }
    //Permet d'essayer de placer une tourelle dans un emplacement donné
    public void TryPlaceTurret(TurretSlot slot) 
    {
        
        if (selectedTurret != null && selectedIndex >= 0 && unlockedTurrets[selectedIndex] &&
            placedTurretsPerType[selectedIndex] < maxTurretsPerType[selectedIndex] &&
            slot.currentTurret == null)
        {
            slot.PlaceTurret(selectedTurret);
            placedTurretsPerType[selectedIndex]++; // Incrémente le compteur de tourelles placées pour ce type
            UpdateTurretButtons(); // Met à jour l'état des boutons (ajout/suppr nb)
        }
    }
    //Appelé lorsqu'une tourelle est retirée d'un emplacement
    public void OnTurretRemoved(int turretTypeIndex)
    {
        placedTurretsPerType[turretTypeIndex] = Mathf.Max(0, placedTurretsPerType[turretTypeIndex] - 1); // Décrémente le compteur de tourelles placées pour ce type
        UpdateTurretButtons();
    }
    //Permet de débloquer une tourelle spécifique
    public void UnlockTurret(int index)
    {
        if (index >= 0 && index < unlockedTurrets.Length)
        {
            unlockedTurrets[index] = true;
            UpdateTurretButtons();
        }
    }
    //Met à jour l'état des boutons de tourelle en fonction des tourelles débloquées et du nombre maximum autorisé
    void UpdateTurretButtons()
    {
        for (int i = 0; i < turretButtons.Length; i++)
        {
            turretButtons[i].interactable = unlockedTurrets[i] && (placedTurretsPerType[i] < maxTurretsPerType[i]);
            if (turretCountTexts != null && i < turretCountTexts.Length)
            {
                //Met à jour le texte pour afficher le nombre de tourelles restantes pouvant être placées
                int restant = maxTurretsPerType[i] - placedTurretsPerType[i];
                turretCountTexts[i].text = restant.ToString();
            }
        }
    }
    //Débloque la prochaine tourelle verrouillée ou augmente la limite d'une tourelle aléatoire si toutes sont déjà débloquées
    public void UnlockNextOrIncreaseLimit()
    {
        // Cherche la première tourelle verrouillée
        for (int i = 0; i < unlockedTurrets.Length; i++)
        {
            // Si elle est verrouillée, la débloque et augmente sa limite
            if (!unlockedTurrets[i])
            {
                unlockedTurrets[i] = true;
                maxTurretsPerType[i]++;
                UpdateTurretButtons();
                return;
            }
        }
        // Si toutes les tourelles sont débloquées, augmente la limite d'une tourelle aléatoirement
        int index = Random.Range(0, unlockedTurrets.Length);
        maxTurretsPerType[index]++;
        UpdateTurretButtons();
    }
}