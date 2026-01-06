using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    public Weapon weapon;
    public int lvl;
    public Gear gear;

    Image imgWeapon;
    Text textlvl;
    Text textName;
    Text textDesc;

    private void Awake()
    {
        imgWeapon = GetComponentsInChildren<Image>()[1];
        imgWeapon.sprite = itemData.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textlvl = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = itemData.itemName;

    }
    
    private void OnEnable()
    {
        textlvl.text = "Lv." + (lvl + 1).ToString();
        switch (itemData.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(itemData.itemDescription, itemData.level_dmg[lvl] * 100, itemData.level_count[lvl]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(itemData.itemDescription, itemData.level_dmg[lvl] * 100);
                break;
            case ItemData.ItemType.Heal:
                textDesc.text = string.Format(itemData.itemDescription, itemData.level_dmg[lvl] * 100);
                break;
            default:
                textDesc.text = string.Format(itemData.itemDescription);
                break;
        }
       
        
    }

    private void LateUpdate()
    {
        textlvl.text = "Lv." + (lvl + 1).ToString();
    }

    public void OnClick()
    {
        switch (itemData.itemType)
        {
            case ItemData.ItemType.Range: //penetration
                if (lvl == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(itemData);
                }
                else
                {
                    float nextRate = itemData.level_dmg[lvl];
                    gear.LevelUp(nextRate);
                }
                lvl++;
                break;

            case ItemData.ItemType.Melee: //damage tower and shovel
                if (lvl == 0) // If the item is at level 0, create a new weapon and initialize it with item data
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(itemData);
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(itemData);
                }
                else  // If the item is at a higher level, level up the existing weapon
                {
                    float nextDmg = itemData.base_dmg;
                    int nextCount = 0;

                    nextDmg += itemData.base_dmg * itemData.level_dmg[lvl];
                    nextCount += itemData.level_count[lvl];

                    weapon.LevelUp(nextDmg, nextCount);

                    float nextRate = itemData.level_dmg[lvl];
                    gear.LevelUp(nextRate);
                }
                lvl++;
                break;

            case ItemData.ItemType.Glove: //fire rate
                if (lvl == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(itemData);
                }
                else
                {
                    float nextRate = itemData.level_dmg[lvl];
                    gear.LevelUp(nextRate);
                }
                lvl++;
                break;

            case ItemData.ItemType.Shoe: //speed
                if (lvl == 0) // If the item is at level 0, create a new weapon and initialize it with item data
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(itemData);
                }
                else  // If the item is at a higher level, level up the existing weapon
                {
                    float nextRate = itemData.level_dmg[lvl];
                    gear.LevelUp(nextRate);
                }
                lvl++;
                break;

            case ItemData.ItemType.Heal:
                // Augmente les maxHP du joueur
                GameManager.instance.maxHP += GameManager.instance.maxHP * itemData.level_dmg[lvl];
                GameManager.instance.HP = GameManager.instance.maxHP;

                // Augmente les maxHP de la tortue
                GameManager.instance.turtleMaxHP += GameManager.instance.turtleMaxHP * itemData.level_dmg[lvl];
                GameManager.instance.turtleHP = GameManager.instance.turtleMaxHP;
                break;
        }
        

        if(lvl == itemData.level_dmg.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}

