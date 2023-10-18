using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Door 
/// </summary>
public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    [SerializeField] private bool isDisabled;
    [SerializeField] private Animator aniOpenDoor;
    [SerializeField] private bool isOpened;

    /// <summary>
    /// Listen to if the targetkey is collected
    /// </summary>
    private void OnEnable()
    {
        Key.OnKeyCollected += TargetKeyCollected;
    }

    private void OnDisable()
    {
        Key.OnKeyCollected -= TargetKeyCollected;
    }

   
    public string InteractionPrompt => prompt;
    public bool IsDisabled => isDisabled;

    [SerializeField] ItemData keyData;

    /// <summary>
    /// Check if the collected key corresponding to this door
    /// </summary>
    /// <param name="item"></param>
    private void TargetKeyCollected(ItemData item)
    {
        if (item == keyData)
        {
            prompt = "Use the Key To Unlock The Door";
        }
    }

    /// <summary>
    /// Door interaction
    /// Handle if key needed/open, close door
    /// </summary>
    /// <param name="interactor">player in this case</param>
    /// <returns>is interaction success</returns>
    public bool Interact(Interactor interactor)
    {
        // Check if there is a target key needed and it is isabled/locked in this case
        if (keyData != null && isDisabled)
        {
            // Check the inventory of the player
            var inventory = interactor.GetComponent<Inventory>();
            if (inventory.inventory != null)
            {
                // Search for the target item
                if (inventory.SearchForItem(keyData))
                {
                    inventory.Remove(keyData);
                    GameManager.Instance.InteractionUISetUp(prompt, IsDisabled);
                    isDisabled = false;
                    prompt = "Open the Door"; // Change the prompt to open the door
                    return true;
                }
                else
                {
                    prompt = "No key Found"; // key needed but not found 
                    return false;
                }
            }
        }
        else // Door is not locked
        {
            if(isOpened) // Chech if is open, and play correponding animation
            {
                isOpened = false;
                aniOpenDoor.Play("CloseDoor", 0, 0.0f);
                prompt = "Open Door";
            }
            else
            {
                isOpened = true;
                aniOpenDoor.Play("OpenDoor", 0, 0.0f);
                prompt = "Close Door";
            }
            return true;
        }
        return false;

    }
}
