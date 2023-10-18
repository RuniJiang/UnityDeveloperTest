using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

/// <summary>
/// Interactable and Collectable Key
/// </summary>
public class Key : MonoBehaviour, IInteractable, ICollectable
{
    [SerializeField] private string prompt;
    [SerializeField] private bool isDisable;

    public string InteractionPrompt => prompt;
    public bool IsDisabled => isDisable;

    public static event HandleKeyCollected OnKeyCollected;
    public delegate void HandleKeyCollected(ItemData item);
    public ItemData keyData; // Key Data, refered to scriptible object

    /// <summary>
    /// Key interact, collect the key into inventory
    /// </summary>
    /// <param name="interactor">player</param>
    /// <returns>true</returns>
    public bool Interact(Interactor interactor)
    {
        Collect();
        return true;
    }

    /// <summary>
    /// Collect the key
    /// </summary>
    public void Collect()
    {
        // Here just destroy the key, but it may better just disable 
        // in the future can be use to drop the key from the inventory 
        Destroy(gameObject); 
        OnKeyCollected?.Invoke(keyData);
    }
}
