using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private void Start()
    {
        items.Add(new Item("Rusty Sword", 0, "The first sword of all adventurers", 1, "Rusty :)", 5, 5, 5, Item.itemtype.Weapon));
        items.Add(new Item("Silver Sword", 1, "A shiny silver sword", 10, "Silver", 6, 7, 6, Item.itemtype.Weapon));
        items.Add(new Item("Golden Sword", 2, "Best sword of all", 100, "Golden", 8, 7, 7, Item.itemtype.Weapon));
        items.Add(new Item("Wooden Mace", 3, "A heavy weapon for starters", 1, "Wooden", 5, 5, 5, Item.itemtype.Weapon));
        items.Add(new Item("Light Armor", 4, "A light armor which protects from basic attacks", 1, "Light", 5, 5, 5, Item.itemtype.Armor));
        items.Add(new Item("Heavy Armor", 5, "A strong armor for warriors.", 10, "Heavy", 6, 6, 6, Item.itemtype.Armor));
        items.Add(new Item("Shiny Armor", 6, "The first sword of all adventurers", 100, "Shiny", 7, 7, 7, Item.itemtype.Armor));
        items.Add(new Item("Health Potion", 7, "The first sword of all adventurers", 1, "HP", 0, 0, 0, Item.itemtype.Consumable));
        items.Add(new Item("Mana Potion", 8, "The first sword of all adventurers", 1, "MP", 0, 0, 0, Item.itemtype.Consumable));
        items.Add(new Item("Strength Potion", 9, "The first sword of all adventurers", 1, "SP", 0, 0, 0, Item.itemtype.Consumable));
        items.Add(new Item("Platinium Sword", 10, "The first sword of all adventurers", 1000, "Platinium", 10, 10, 10, Item.itemtype.Weapon));
        items.Add(new Item("Golden Mace", 11, "The first sword of all adventurers", 200, "Golden Mace", 8, 9, 8, Item.itemtype.Weapon));
        items.Add(new Item("Orc Armor", 12, "The first sword of all adventurers", 200, "Orc Armor", 8, 7, 8, Item.itemtype.Armor));
    }
}
