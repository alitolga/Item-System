using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    public int inventorySize;
    private bool showInventory;
    private bool showInfoBox;
    private string info;
    private Item infoItem;
    private Rect infoRect;
    public Item draggedItem;
    public int draggedIndex;
    public bool draggingItem;
    private Item[] craftItems = new Item[3];

    void Start ()
    { 
        draggingItem = false;
        infoItem = new Item();
        draggedItem = new Item();
        infoRect = new Rect();
        inventorySize = 3;
        showInfoBox = false;
        showInventory = true;

        craftItems[0] = new Item();
        craftItems[0].name = "Dust";
        craftItems[0].id = 13;
        craftItems[0].icon = "Dust";
        craftItems[0].description = "An item which is generally useless for crafting";
        craftItems[0].type = Item.itemtype.Crafting;
        craftItems[1] = new Item();
        craftItems[1].name = "Metal";
        craftItems[1].id = 14;
        craftItems[1].icon = "Metal";
        craftItems[1].description = "A crafting item for almost everything";
        craftItems[1].type = Item.itemtype.Crafting;
        craftItems[2] = new Item();
        craftItems[2].name = "Rare Gem";
        craftItems[2].id = 15;
        craftItems[2].icon = "Gem";
        craftItems[2].description = "A non-common precious crafting item";
        craftItems[2].type = Item.itemtype.Crafting;

        for (int i = 0; i < inventorySize * inventorySize; i++)
        {
            Item temp = new Item();
            inventory.Add(temp);
        }
        for (int i = 0; i < 6; i++)
        {
            Item temp = new Item();
            inventory.Add(temp);
        }
    }
	
	void Update ()
    {
        if (Input.GetKeyDown("i") || Input.GetKeyDown("2"))
        {
            showInventory = !showInventory;
        }

    }

    void OnGUI()
    {
        info = "";
        if (showInventory)
        {
            ShowInventory();
        }

        if (draggingItem)
        {
            DrawIcon();
        }

    }

    void DrawIcon()
    {
        int iconBoxWidth = 60;
        int iconBoxHeight = 30;
        Rect iconRect = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, iconBoxWidth, iconBoxHeight);
        GUI.Box(iconRect, draggedItem.icon);
    }

    void ShowInventory()
    {
        // Variavles.
        int backgroundWidth = 265;
        int backgroundHeight = 265;
        int backgroundXOrigin = 350;
        int backgroundYOrigin = 40;
        Rect backgroundRect = new Rect(backgroundXOrigin, backgroundYOrigin, backgroundWidth, backgroundHeight);
        GUI.Box(backgroundRect, "");
        int boxWidth = 75;
        int boxHeight = 60;
        int xOrigin = 360;
        int yOrigin = 50;
        ItemBank itemBank = GameObject.FindGameObjectWithTag("Item Bank").GetComponent<ItemBank>();
        Event e = Event.current;

        // Drawing the inventory slots.
        for (int i = 0; i < inventorySize; i++)
        {
            for (int j = 0; j < inventorySize; j++)
            {
                int index = 3 * i + j;
                Rect slotRect = new Rect(xOrigin, yOrigin, boxWidth, boxHeight);
                GUI.Box(slotRect, inventory[index].icon);

                if (slotRect.Contains(e.mousePosition))
                {
                    // If I click inside a rectangle, I see the item info.
                    if (e.button == 0 && e.type == EventType.mouseDown)
                    {
                        showInfoBox = !showInfoBox;
                        infoItem = inventory[index];
                        infoRect = slotRect;
                    }

                    // If I'm not dragging an item, I start dragging an item when I click and drag.
                    if (e.button == 0 && e.type == EventType.mouseDrag && !draggingItem && !itemBank.draggingItem)
                    {
                        draggedItem = inventory[index];
                        draggedIndex = index;
                        draggingItem = true;
                    }

                    // When I release left mouse button.
                    if (e.button == 0 && e.type == EventType.mouseUp)
                    {
                        // If I have been dragging from the bank.
                        if(itemBank.draggingItem)
                        {
                            // If it's an empty slot, take/buy it.
                            if (inventory[index].id == -1)
                            {
                                ItemCreator itemfunctions = GameObject.FindGameObjectWithTag("Item Creator").GetComponent<ItemCreator>();
                                itemfunctions.copyItems(inventory[index], itemBank.draggedItem);
                                // inventory[index] = itemBank.draggedItem;
                            }
                            itemBank.draggingItem = false;
                        }

                        // If I have been dragging from my Inventory.
                        if(draggingItem)
                        {
                            // If I'm dragging an empty item, simply release it.
                            if(draggedItem.id == -1)
                            {
                                draggedItem = null;
                                draggingItem = false;
                            }

                            // Else, switch the items.
                            else
                            {
                                inventory[draggedIndex] = inventory[index];
                                inventory[index] = draggedItem;
                                draggedItem = null;
                                draggingItem = false;
                            }
                            
                        }
                    }
                }

                xOrigin += boxWidth + 10;
            }
            xOrigin = 360;
            yOrigin += boxHeight + 10;
        }

        // Drawing the upgrade, disenchant and remove areas.
        boxHeight /= 2;
        string action = "";  
        for (int i = 0; i < inventorySize; i++)
        {
            if (i == 0) action = "Upgrade";
            else if (i == 1) action = "Disenchant";
            else action = "Remove";
            Rect slotRect = new Rect(xOrigin, yOrigin, boxWidth, boxHeight);
            GUI.Box(slotRect, action);
            if(draggingItem && slotRect.Contains(Event.current.mousePosition) && Event.current.button == 0 && Event.current.type == EventType.mouseUp)
            {
                if(action == "Upgrade")
                {
                    if (draggedItem.type == Item.itemtype.Armor || draggedItem.type == Item.itemtype.Weapon)
                    {
                        inventory[draggedIndex].stat1 += 1;
                        inventory[draggedIndex].stat2 += 1;
                        inventory[draggedIndex].stat3 += 1;
                        draggedItem = null;
                        draggingItem = false;
                        print("Item has been upgraded");
                    }
                }
                if (action == "Disenchant")
                {
                    if (draggedItem.type == Item.itemtype.Armor || draggedItem.type == Item.itemtype.Weapon || draggedItem.type == Item.itemtype.Consumable)
                    {
                        int k = Random.Range(0, 3);
                        if (k == 2) { k = Random.Range(0, 3); }
                        inventory[draggedIndex] = craftItems[k];
                        draggedItem = null;
                        draggingItem = false;
                        print("Item has been disenchanted");
                    }
                }
                if (action == "Remove")
                {
                    inventory[draggedIndex] = new Item();
                    draggedItem = null;
                    draggingItem = false;
                    print("Item has been removed");
                }
            }
            xOrigin += boxWidth + 10;
        }

        // Draw the equipped items
        boxHeight *= 2;
        boxHeight += 15;
        string header = "";
        yOrigin = backgroundYOrigin;
        for (int i = 0; i < 3; i++)
        {
            if (i == 0) header = "Weapon";
            else if (i == 1) header = "Armor";
            else header = "Consumable";
            Rect slotRect = new Rect(xOrigin, yOrigin, boxWidth, boxHeight);
            GUI.Box(slotRect, header);
            xOrigin += boxWidth;
        }

        xOrigin -= 3 * boxWidth;
        yOrigin += boxHeight;
        for (int i = 9; i < 15; i++)
        {
            if(i == 12)
            {
                yOrigin += boxHeight;
                xOrigin -= 3 * boxWidth;
            }
            int index = i;

            Rect slotRect = new Rect(xOrigin, yOrigin, boxWidth, boxHeight);
            GUI.Box(slotRect, inventory[index].icon);

            if (slotRect.Contains(Event.current.mousePosition))
            {
                // If I click inside a rectangle, I see the item info.
                if (e.button == 0 && e.type == EventType.mouseDown)
                {
                    showInfoBox = !showInfoBox;
                    infoItem = inventory[index];
                    infoRect = slotRect;
                }

                // If I'm not dragging an item, I start dragging an item when I click and drag.
                if (e.button == 0 && e.type == EventType.mouseDrag && !draggingItem && !itemBank.draggingItem)
                {
                    draggedItem = inventory[index];
                    draggedIndex = index;
                    draggingItem = true;
                }

                // When I release left mouse button.
                if (e.button == 0 && e.type == EventType.mouseUp)
                {
                    // If I have been dragging from the bank.
                    /* if (itemBank.draggingItem)
                    {
                        // If it's an empty slot, take/buy it.
                        if (inventory[index].id == -1)
                        {
                            ItemCreator itemfunctions = GameObject.FindGameObjectWithTag("Item Creator").GetComponent<ItemCreator>();
                            itemfunctions.copyItems(inventory[index], itemBank.draggedItem)
                                // inventory[index] = itemBank.draggedItem;
                            }
                        itemBank.draggingItem = false;
                    } */

                    // If I have been dragging from my Inventory.
                    if (draggingItem)
                    {
                        // If I'm dragging an empty item, simply release it.
                        if (draggedItem.id == -1)
                        {
                            draggedItem = null;
                            draggingItem = false;
                        }

                        // Else, switch the items.
                        else
                        {
                            if (index % 3 == 0)
                            {
                                if(draggedItem.type == Item.itemtype.Weapon)
                                {
                                    inventory[draggedIndex] = inventory[index];
                                    inventory[index] = draggedItem;
                                    draggedItem = null;
                                    draggingItem = false;
                                }
                            }
                            if (index % 3 == 1)
                            {
                                if (draggedItem.type == Item.itemtype.Armor)
                                {
                                    inventory[draggedIndex] = inventory[index];
                                    inventory[index] = draggedItem;
                                    draggedItem = null;
                                    draggingItem = false;
                                }
                            }
                            if (index % 3 == 2)
                            {
                                if (draggedItem.type == Item.itemtype.Consumable)
                                {
                                    inventory[draggedIndex] = inventory[index];
                                    inventory[index] = draggedItem;
                                    draggedItem = null;
                                    draggingItem = false;
                                }
                            }
                            
                        }

                    }
                }
            }
            xOrigin += boxWidth;
        }

        // If I release outside a rectangle, do nothing.
        if (e.button == 0 && e.type == EventType.mouseUp)
        {
            draggedItem = null;
            draggingItem = false;
        }

        // If I clicked inside a box to see the item info.
        if (showInfoBox)
        {
            info = CreateInfo(infoItem);
            // If the item is not empty and I'm inside the item box, show the item info.
            if (infoItem.id != -1)
            {
                ShowInfoBox(infoItem, info);
            }
            if (!infoRect.Contains(e.mousePosition))
            {
                showInfoBox = !showInfoBox;
            }
        }

    }

    // Create the info string for the item.
    string CreateInfo(Item item)
    {
        string info = item.name + "\nLevel: " + item.level + "\nType: " + item.type;
        info += "\nStat1: " + item.stat1 + "\nStat2: " + item.stat2 + "\nStat3: " + item.stat3;
        info += '\n' + item.description;
        return info;
    }

    // Draw the item info with the info string.
    void ShowInfoBox(Item item, string info)
    {
        int boxWidth = 120;
        int boxHeight = 120;
        Rect infoRect = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, boxWidth, boxHeight);
        GUI.Box(infoRect, info);
    }

}
