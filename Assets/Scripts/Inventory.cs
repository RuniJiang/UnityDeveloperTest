using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Inventory system
/// Should be attached to the player
/// </summary>
public class Inventory : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private void OnEnable()
    {
        // Subscribe to the collectible collected
        Key.OnKeyCollected += Add;
        MatchlockGun.OnMatchlockGunCollected += Add;
    }

    private void OnDisable()
    {
        Key.OnKeyCollected -= Add;
        MatchlockGun.OnMatchlockGunCollected -= Add;
    }

    /// <summary>
    ///  Add the item to the invnentory
    ///  The item need to have scriptable object reference
    /// </summary>
    /// <param name="itemData">Item data(scriptable object)</param>
    public void Add(ItemData  itemData)
    {
        // Try to get the value in the dictionary
        if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            // Has item in the dictionary
            item.AddToStack();
            Debug.Log("add to stack");
        }
        // Item is not in dictionary, add item to dic
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
        }
        GameManager.Instance.ShowItemInInventory();
    }

    /// <summary>
    /// Remove the item from inventory
    /// </summary>
    /// <param name="itemData"></param>
    public void Remove(ItemData itemData)
    {
        
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveFromStack();
            // If no more this item, remove the item from the dictionary
            if(item.stackSize == 0)
            {
                inventory.Remove(item);
            }
        }
        GameManager.Instance.ShowItemInInventory();
    }

    /// <summary>
    /// Check if the item is in the dictionary
    /// </summary>
    /// <param name="itemData">itemData</param>
    /// <returns>is in the Dictionary</returns>
    public bool SearchForItem(ItemData itemData)
    {
        bool isFound = false;
        if(itemDictionary.TryGetValue(itemData, out InventoryItem _item))
        {
            isFound = true;
        }

        return isFound;
    }
}
