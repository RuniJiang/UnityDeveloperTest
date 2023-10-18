using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheel : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    [SerializeField] private bool isDisabled;
    [SerializeField] private ItemData unlockCondition;

    private bool isForceUsed = false;   // if player use the wheel with gun
    private bool isUsedWithGun = false;
    public bool IsForceUsed { get { return isForceUsed; } }
    public bool IsUsedWithGun { get { return isUsedWithGun; } }

    public string InteractionPrompt => prompt;
    public bool IsDisabled => isDisabled;

    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private DialogueTrigger dialogueTriggerwithGun;

    /// <summary>
    /// Steerwheel interaction
    /// Check the player interact with it with/without key
    /// Lead to ending
    /// </summary>
    /// <param name="interactor"></param>
    /// <returns></returns>
    public bool Interact(Interactor interactor)
    {
        // With the unlock condition
        if (unlockCondition != null)
        {
            var inventory = interactor.GetComponent<Inventory>();

            if (inventory == null) return false;
            if (inventory.inventory != null)
            {
                // Based on if the player has the gun
                // Set the ending bool used in the game manager
                if (inventory.SearchForItem(unlockCondition))
                {
                    dialogueTriggerwithGun.TriggerDialogue();
                    isUsedWithGun = true;
                    return true;
                }
                else
                {
                    dialogueTrigger.TriggerDialogue();
                    isForceUsed = true;
                    return true;
                }
            }
        }
        return false;
    }
}
