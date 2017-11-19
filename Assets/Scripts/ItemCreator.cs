using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreator : MonoBehaviour {

    public Item newItem = new Item();
    public bool showCreator;
    public string t;
    public int ID;
   
    void Start ()
    {
        showCreator = true;
        ID = 999;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c") || Input.GetKeyDown("3"))
        {
            showCreator = !showCreator;
            print(newItem.name);
            print(newItem.level);
        }
    }

    void OnGUI()
    {
        if (showCreator)
        {
            
            int xOrigin = 0;
            int yOrigin = 0;
            int height = Screen.height/18;
            int width = 2*(Screen.width)/13;

            Rect iconRect = new Rect(xOrigin, yOrigin, width, height);
            GUI.Box(iconRect, "Item Creation");
            yOrigin += height;

            Rect textRect = new Rect(xOrigin, yOrigin, width, height);
            GUI.Box(textRect, "Item name");
            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);
            newItem.name = GUI.TextArea(textRect, newItem.name);

            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);
            GUI.Box(textRect, "Item description");
            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);
            newItem.description = GUI.TextArea(textRect, newItem.description);

            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);
            GUI.Box(textRect, "Item level");
            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);
            Int32.TryParse(GUI.TextArea(textRect, newItem.level.ToString()), out newItem.level);

            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);
            GUI.Box(textRect, "Item icon");
            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);
            newItem.icon = GUI.TextArea(textRect, newItem.icon);

            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);
            GUI.Box(textRect, "Stat1, Stat2 and Stat3");
            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);
            Int32.TryParse(GUI.TextArea(textRect, newItem.stat1.ToString()), out newItem.stat1);
            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);
            Int32.TryParse(GUI.TextArea(textRect, newItem.stat2.ToString()), out newItem.stat2);
            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);
            Int32.TryParse(GUI.TextArea(textRect, newItem.stat3.ToString()), out newItem.stat3);

            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);
            GUI.Box(textRect, "Item Type");
            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);

            t = GUI.TextArea(textRect, t);
            if (string.Compare(t, "weapon", true) == 0) newItem.type = Item.itemtype.Weapon;
            else if (string.Compare(t, "armor", true) == 0) newItem.type = Item.itemtype.Armor;
            else if (string.Compare(t, "consumable", true) == 0) newItem.type = Item.itemtype.Consumable;
            else if (string.Compare(t, "crafting", true) == 0) newItem.type = Item.itemtype.Crafting;
            else newItem.type = Item.itemtype.None;

            // Create button
            InventoryController inv = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryController>();
            yOrigin += height;
            textRect = new Rect(xOrigin, yOrigin, width, height);
            GUI.Box(textRect, "Create");
            bool added = false;
            if(textRect.Contains(Event.current.mousePosition) && Event.current.button == 0 && Event.current.type == EventType.mouseDown)
            {
                newItem.id = ID;
                for (int i = 0; i < (inv.inventorySize)*(inv.inventorySize); i++)
                {
                    if (inv.inventory[i].id == -1)
                    {
                        Item addedItem = new Item();
                        copyItems(addedItem, newItem);
                        if (addedItem.id >= 999)
                        {
                            inv.inventory[i] = addedItem;
                            added = true;
                            ID++;
                        }
                        else
                        {
                            print("Please fill all the values correctly");
                        }

                        break;
                    }
                }
            }
            if (added) print("Item has been added");

        }
    }

    public void copyItems(Item destination, Item source)
    {
        destination.icon = source.icon;
        destination.level = source.level;
        destination.description = source.description;
        destination.name = source.name;
        destination.stat1 = source.stat1;
        destination.stat2 = source.stat2;
        destination.stat3 = source.stat3;
        destination.type = source.type;
        if(source.description != "" && source.name != "" && source.icon != "" && source.type != Item.itemtype.None && source.level > 0 && source.stat1 >= 0 && source.stat2 >= 0 && source.stat3 >= 0)
        {
            destination.id = source.id;
        }
        else
        {
            destination.id = -1;
        }
    }


}
