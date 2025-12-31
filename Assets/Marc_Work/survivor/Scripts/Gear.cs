using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        //Basic set
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        //Property set
        type = data.itemType;
        rate = data.level_dmg[0];
        ApplyGear();

    }
    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }

    }




    private void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        foreach (Weapon w in weapons)
        {
            switch(w.id)
            {
                case 0:
                    w.speed = 300 + (300 * rate);
                    break;
                default:
                    w.speed = 0.5f + (1f * rate);
                    break;
            }
        }

        // Pour toutes les tours de la scène
        Tower_shoot[] towers = FindObjectsOfType<Tower_shoot>();
        foreach (Tower_shoot t in towers)
        {
            float baseRate = 1f; // Remplace par la valeur de base de ta tour
            float newRate = baseRate * (1f - rate); // Plus le rate est haut, plus la cadence est rapide
            t.SetFireRate(newRate);
        }
    }

    void SpeedUp()
    {
        float speed = 6;
        GameManager.instance.player.speed = speed + (speed * rate);


    }
}


