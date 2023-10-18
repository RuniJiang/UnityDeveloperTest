using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Inventory Item
/// </summary>
[Serializable]
public class InventoryItem
{
    public ItemData itemData; // Item data (scriptableObject)
    public int stackSize;     // stack size for the item in inventory

    public InventoryItem(ItemData item)
    { 
        itemData= item;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack() 
    {
        stackSize--;
    }

}
