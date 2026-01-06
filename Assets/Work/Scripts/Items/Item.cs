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
        //Change la description en fonction du niveau de l'item
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
    //Méthode appelée lors du clic sur l'item dans l'inventaire
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

            case ItemData.ItemType.Melee: //damage tower et fourche
                if (lvl == 0) // Si l'item est au niveau 0, créer une nouvelle arme et l'initialiser avec les données de l'item
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(itemData);
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(itemData);
                }
                else  // Si l'item est à un niveau supérieur, faire évoluer l'arme existante
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

