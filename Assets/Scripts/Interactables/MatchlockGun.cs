using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collectable and Interactable gun
/// </summary>
public class MatchlockGun : MonoBehaviour, IInteractable, ICollectable
{

    [SerializeField] private string prompt;

    public string InteractionPrompt => prompt;
    [SerializeField] private bool isLocked;
    public bool IsDisabled => isLocked;


    public static event HandleMatchlockGunCollected OnMatchlockGunCollected;
    public delegate void HandleMatchlockGunCollected(ItemData item);
    public ItemData itemData;

    /// <summary>
    /// Interact -> collect
    /// </summary>
    /// <param name="interactor"></param>
    /// <returns></returns>
    public bool Interact(Interactor interactor)
    {
        Collect();
        return true;
    }

    /// <summary>
    /// Collect the gun to inventory
    /// </summary>
    public void Collect()
    {
        Debug.Log("Collect Matchlock gun");
        Destroy(gameObject);
        OnMatchlockGunCollected?.Invoke(itemData);
    }

}
