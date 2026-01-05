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
        if (index >= 0 && index < turretPrefabs.Length && unlockedTurrets[index])
        {
            selectedTurret = turretPrefabs[index];
            selectedIndex = index;
            AudioManager.instance.PlaySFX(AudioManager.SFX.Select);
        }
    }

    public void TryPlaceTurret(TurretSlot slot)
    {
        if (selectedTurret != null && selectedIndex >= 0 && unlockedTurrets[selectedIndex] &&
            placedTurretsPerType[selectedIndex] < maxTurretsPerType[selectedIndex] &&
            slot.currentTurret == null)
        {
            slot.PlaceTurret(selectedTurret);
            placedTurretsPerType[selectedIndex]++;
            UpdateTurretButtons();
        }
    }

    public void OnTurretRemoved(int turretTypeIndex)
    {
        placedTurretsPerType[turretTypeIndex] = Mathf.Max(0, placedTurretsPerType[turretTypeIndex] - 1);
        UpdateTurretButtons();
    }

    public void UnlockTurret(int index)
    {
        if (index >= 0 && index < unlockedTurrets.Length)
        {
            unlockedTurrets[index] = true;
            UpdateTurretButtons();
        }
    }

    void UpdateTurretButtons()
    {
        for (int i = 0; i < turretButtons.Length; i++)
        {
            turretButtons[i].interactable = unlockedTurrets[i] && (placedTurretsPerType[i] < maxTurretsPerType[i]);
            if (turretCountTexts != null && i < turretCountTexts.Length)
            {
                int restant = maxTurretsPerType[i] - placedTurretsPerType[i];
                turretCountTexts[i].text = restant.ToString();
            }
        }
    }

    public void UnlockNextOrIncreaseLimit()
    {
        for (int i = 0; i < unlockedTurrets.Length; i++)
        {
            if (!unlockedTurrets[i])
            {
                unlockedTurrets[i] = true;
                maxTurretsPerType[i]++;
                UpdateTurretButtons();
                return;
            }
        }
        int index = Random.Range(0, unlockedTurrets.Length);
        maxTurretsPerType[index]++;
        UpdateTurretButtons();
    }
}