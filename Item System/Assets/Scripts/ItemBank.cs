using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBank : MonoBehaviour
{

    public List<Item> bank = new List<Item>();
    private ItemDatabase itemdatabase;
    private int banksize;
    private List<Item> slots = new List<Item>();
    private bool showbank;
    private bool showInfoBox;
    private string info;
    public bool draggingItem;
    public Item draggedItem;
    private Item infoItem;
    private Rect infoRect;

    void Start()
    {
        draggingItem = false;
        draggedItem = new Item();
        infoItem = new Item();
        infoRect = new Rect();
        showInfoBox = false;
        showbank = false;
        banksize = 3;
        itemdatabase = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
        for (int i = 0; i < banksize * banksize; i++)
        {
            Item bankItem = new Item();
            bankItem = itemdatabase.items[i];
            bank.Add(bankItem);
            slots.Add(bankItem);
        }
    }

    void Update()
    {
        
        if (Input.GetKeyDown("b") || Input.GetKeyDown("1"))
        {
            showbank = !showbank;
        }
        
    }

    void OnGUI()
    {
        info = "";
        if (showbank)
        {
            ShowBank();
        }

        // Draw the item which is beinf dragged.
        if(draggingItem)
        {
            DrawIcon();
        }

        // After the release of the mouse button there won't be any dragged items.
        if(Event.current.button == 0 && Event.current.type == EventType.mouseUp)
        {
            draggedItem = null;
            draggingItem = false;
        }

    }

    // Draws the item icon that's being dragged.
    void DrawIcon()
    {
        int iconBoxWidth = 60;
        int iconBoxHeight = 30;
        Rect iconRect = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, iconBoxWidth, iconBoxHeight);
        GUI.Box(iconRect, draggedItem.icon);
    }

    void ShowBank()
    {

        // Variables
        int backgroundWidth = 220;
        int backgroundHeight = 220;
        int backgroundXOrigin = 60;
        int backgroundYOrigin = 60;
        Rect backgroundRect = new Rect(backgroundXOrigin, backgroundYOrigin, backgroundWidth, backgroundHeight);
        GUI.Box(backgroundRect, "");
        int boxWidth = 60;
        int boxHeight = 60;
        int xOrigin = 70;
        int yOrigin = 70;
        InventoryController inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryController>();
        Event e = Event.current;

        // Drawing bank items.
        for (int i = 0; i < banksize; i++)
        {
            for (int j = 0; j < banksize; j++)
            {
                int index = 3 * i + j;
                Rect slotRect = new Rect(xOrigin, yOrigin, boxWidth, boxHeight);
                GUI.Box(slotRect, bank[index].icon);

                if (slotRect.Contains(e.mousePosition))
                {
                    // If I clicked within an item box, show the item info.
                    if (e.button == 0 && e.type == EventType.mouseDown)
                    {
                        showInfoBox = !showInfoBox;
                        infoItem = bank[index];
                        infoRect = slotRect;
                    }
                    
                    // If I'm not dragging any items, drag the item.
                    if(e.button == 0 && e.type == EventType.mouseDrag && !draggingItem && !inventory.draggingItem)
                    {
                        draggedItem = bank[index];
                        draggingItem = true;
                    }

                }
                xOrigin += boxWidth + 10;
            }
            xOrigin = 70;
            yOrigin += boxHeight + 10;
        }

        // Draw the item info box.
        if (showInfoBox)
        {
            info = CreateInfo(infoItem);
            // If the item is not empty and I'm within the item box, show the item info.
            if (infoItem.id != -1)
            {
                ShowInfoBox(infoItem, info);
            }
            if (!infoRect.Contains(e.mousePosition))
            {
                showInfoBox = !showInfoBox;
            }
        }

        /*
        // Draw upgrade area
        int upgradeBoxWidth = 90;
        int upgradeBoxHeight = 30;
        Rect upgradeRect = new Rect(xOrigin, yOrigin, upgradeBoxWidth, upgradeBoxHeight);
        string upgrade = "Upgrade";
        GUI.Box(upgradeRect, upgrade);

        // Draw disenchant area
        xOrigin += 90;
        int disenchantBoxWidth = 90;
        int disenchantBoxHeight = 30;
        Rect disenchantRect = new Rect(xOrigin, yOrigin, disenchantBoxWidth, disenchantBoxHeight);
        string disenchant = "Disenchant";
        GUI.Box(disenchantRect, disenchant);
        */

    }

    // Creating the info string. (The string to be displayed when clicked on an item.)
    string CreateInfo(Item item)
    {
        string info = item.name + "\nLevel: " + item.level + "\nType: " + item.type;
        info += "\nStat1: " + item.stat1 + "\nStat2: " + item.stat2 + "\nStat3: " + item.stat3;
        return info;
    }

    // Draws the info box with the info string.
    void ShowInfoBox(Item item, string info)
    {
        int boxWidth = 120;
        int boxHeight = 120;
        Rect infoRect = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, boxWidth, boxHeight);
        GUI.Box(infoRect, info);
    }


}
