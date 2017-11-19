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
        showbank = true;
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
        int iconBoxWidth = Screen.height / 6;
        int iconBoxHeight = Screen.height / 12;
        Rect iconRect = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, iconBoxWidth, iconBoxHeight);
        GUI.Box(iconRect, draggedItem.icon);
    }

    void ShowBank()
    {

        // Variables
        int backgroundXOrigin = 9*Screen.width/52;
        int backgroundYOrigin = 0;
        int boxWidth = Screen.width/13;
        int boxHeight = Screen.height/6;
        int xOrigin = backgroundXOrigin;
        int yOrigin = backgroundYOrigin;
        InventoryController inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryController>();
        Event e = Event.current;

        Rect iconRect = new Rect(xOrigin, yOrigin, boxWidth, boxHeight/2);
        GUI.Box(iconRect, "Item Bank");
        yOrigin += boxHeight / 2;
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
                xOrigin += boxWidth;
            }
            xOrigin = backgroundXOrigin;
            yOrigin += boxHeight;
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

    }

    // Creating the info string. (The string to be displayed when clicked on an item.)
    string CreateInfo(Item item)
    {
        string info = item.name + "\nLevel: " + item.level + "\nType: " + item.type;
        info += "\nStat1: " + item.stat1 + "\nStat2: " + item.stat2 + "\nStat3: " + item.stat3;
        info += '\n' + item.description;
        return info;
    }

    // Draws the info box with the info string.
    void ShowInfoBox(Item item, string info)
    {
        int boxWidth = 2*Screen.width/13;
        int boxHeight = Screen.height/2;
        Rect infoRect = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, boxWidth, boxHeight);
        GUI.Box(infoRect, info);
    }


}
