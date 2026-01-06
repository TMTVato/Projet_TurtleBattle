using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }
    // Affiche level-up UI
    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        AudioManager.instance.PlaySFX(AudioManager.SFX.LevelUp);
        AudioManager.instance.HighPassBGM(true);
    }
    // Cache level-up UI
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioManager.instance.PlaySFX(AudioManager.SFX.Select);
        AudioManager.instance.HighPassBGM(false);
    }
    //Sélectionne un item
    public void Select(int index)
    {
        items[index].OnClick();
        Hide();
    }

    // Prépare les items à afficher lors du level-up
    private void Next()
    {
        // 1. Désactive tous les items
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. Liste des indices d'items non max
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].lvl < items[i].itemData.level_dmg.Length)
                availableIndices.Add(i);
        }

        // 3. Cherche l'index d'un item consommable
        int consumableIndex = -1;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemData.itemType == ItemData.ItemType.Heal) // ou ton type de consommable
            {
                consumableIndex = i;
                break;
            }
        }

        // 4. Sélectionne jusqu'à 3 indices
        List<int> selectedIndices = new List<int>();
        while (selectedIndices.Count < 3)
        {
            if (availableIndices.Count > 0)
            {
                int randIdx = availableIndices[Random.Range(0, availableIndices.Count)];
                selectedIndices.Add(randIdx);
                availableIndices.Remove(randIdx);
            }
            else if (consumableIndex != -1 && !selectedIndices.Contains(consumableIndex))
            {
                selectedIndices.Add(consumableIndex);
            }
            else
            {
                break; // tous les choix sont max ou pas de consommable
            }
        }

        // 5. Active les items sélectionnés
        foreach (int idx in selectedIndices)
        {
            items[idx].gameObject.SetActive(true);
        }
    }

}
