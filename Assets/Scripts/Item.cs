using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stat
{
    string name;
    int value;
}

public class Item
{
    public string name;
    public int id;
    public string description;
    public int level;
    public string icon;
    public int stat1;
    public int stat2;
    public int stat3;
    public itemtype type;
    public enum itemtype
    {
        Weapon, Armor, Consumable, None, Crafting
    }

    public Item(string itemName, int itemId, string itemDescription, int itemLevel, string itemIcon, int itemStat1, int itemStat2, int itemStat3, itemtype itemType)
    {
        name = itemName;
        id = itemId;
        description = itemDescription;
        level = itemLevel;
        icon = itemIcon;
        stat1 = itemStat1;
        stat2 = itemStat2;
        stat3 = itemStat3;
        type = itemType;
    }

    public Item()
    {
        name = "";
        id = -1;
        description = "";
        level = -1;
        icon = "";
        stat1 = -1;
        stat2 = -1;
        stat3 = -1;
        type = itemtype.None;
    }

}
