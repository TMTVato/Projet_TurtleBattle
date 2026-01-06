// C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        //Set basique
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        //Set propriétés
        type = data.itemType;
        rate = data.level_dmg[0];
        ApplyGear();
    }
    
    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }
    //Applique les bonus de l'équipement en fonction de son type
    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Melee:
                DamageUp();
                break;
            case ItemData.ItemType.Glove:
                FireRateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
            case ItemData.ItemType.Range:
                PenetrationUp();
                RangeUp();
                break;
        }
    }
    //Augmente les damages des tours 
    private void DamageUp()
    {
        GameManager.instance.bonusDamage = rate;
        Tower_shoot[] towers = FindObjectsOfType<Tower_shoot>();
        foreach (Tower_shoot t in towers)
        {
            t.ApplyBonuses(
                GameManager.instance.bonusDamage,
                GameManager.instance.bonusFireRate,
                GameManager.instance.bonusPenetration,
                GameManager.instance.bonusSpeed,
                GameManager.instance.bonusRange
            );
        }
    }
    //Augmente le fire rate des tours
    private void FireRateUp()
    {
        GameManager.instance.bonusFireRate = rate;
        Tower_shoot[] towers = FindObjectsOfType<Tower_shoot>();
        foreach (Tower_shoot t in towers)
        {
            t.ApplyBonuses(
                GameManager.instance.bonusDamage,
                GameManager.instance.bonusFireRate,
                GameManager.instance.bonusPenetration,
                GameManager.instance.bonusSpeed,
                GameManager.instance.bonusRange
            );
        }
    }
    //Augmente la penetration des tours
    private void PenetrationUp()
    {
        GameManager.instance.bonusPenetration = rate;
        Tower_shoot[] towers = FindObjectsOfType<Tower_shoot>();
        foreach (Tower_shoot t in towers)
        {
            t.ApplyBonuses(
                GameManager.instance.bonusDamage,
                GameManager.instance.bonusFireRate,
                GameManager.instance.bonusPenetration,
                GameManager.instance.bonusSpeed,
                GameManager.instance.bonusRange
            );
        }
    }
    //Augmente la vitesse du joueur
    void SpeedUp()
    {
        float speed = 6;
        GameManager.instance.player.speed = speed + (speed * rate);
        Tower_shoot[] towers = FindObjectsOfType<Tower_shoot>();
        GameManager.instance.bonusSpeed = rate;
        foreach (Tower_shoot t in towers)
        {
            t.ApplyBonuses(
                GameManager.instance.bonusDamage,
                GameManager.instance.bonusFireRate,
                GameManager.instance.bonusPenetration,
                GameManager.instance.bonusSpeed,
                GameManager.instance.bonusRange
            );
        }
    }

    //Augmente la portée des tours
    private void RangeUp()
    {
        GameManager.instance.bonusRange = rate;
        Tower_shoot[] towers = FindObjectsOfType<Tower_shoot>();
        foreach (Tower_shoot t in towers)
        {
            t.ApplyBonuses(
                GameManager.instance.bonusDamage,
                GameManager.instance.bonusFireRate,
                GameManager.instance.bonusPenetration,
                GameManager.instance.bonusSpeed,
                GameManager.instance.bonusRange
            );
        }
    }
}
