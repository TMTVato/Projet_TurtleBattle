using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    
    public enum ItemType
    {
        Melee, Range, Glove, Shoe, Heal
    }
    //Gère les données des items (armes et gears)
    [Header("Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea(3, 10)]
    public string itemDescription;
    public Sprite itemIcon;


    //Statistiques par niveau
    [Header("Level Data")]
    public float base_dmg;
    public int base_count;
    public float[] level_dmg;
    public int[] level_count;
    //Info Arme si besoin
    [Header("Weapon Info")]

    public GameObject proj;


}
