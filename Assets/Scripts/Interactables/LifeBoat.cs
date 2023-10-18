using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum LifeboatState
{ 
    KeyNeeded,
    UseLever,
    LowerTheBoat,
    UseLifeboat
}
public class LifeBoat : MonoBehaviour, IInteractable
{
    // Dictionary to map LifeboatState to interaction prompts
    private Dictionary<LifeboatState, string> interactionPrompt = new Dictionary<LifeboatState, string>();
    [SerializeField] private LifeboatState curBoatState;
    [SerializeField] private string keyNeededPrompt;
    [SerializeField] private string useLeverPrompt;
    [SerializeField] private string lowerTheBoatPrompt;
    [SerializeField] private string useLifeboatPrompt;

    [SerializeField] private Collider boatUseCollider;

    private string prompt;

    public string InteractionPrompt => prompt;
    [SerializeField] private bool isLocked;
    public bool IsDisabled => isLocked;

    [SerializeField] private ItemData unlockCondition; // Target key

    public static event HandleUseKey OnKeyUsedForBoat;
    public delegate void HandleUseKey();

    private void Start()
    {
        // Initialize the interaction prompts and state
        interactionPrompt.Add(LifeboatState.KeyNeeded, keyNeededPrompt);
        interactionPrompt.Add(LifeboatState.UseLever, useLeverPrompt);
        interactionPrompt.Add(LifeboatState.LowerTheBoat, lowerTheBoatPrompt);
        interactionPrompt.Add(LifeboatState.UseLifeboat, useLifeboatPrompt);
        curBoatState = LifeboatState.KeyNeeded;
        prompt = interactionPrompt[curBoatState];
        isLocked= true;
        //boatUseCollider.enabled= false;
    }

    /// <summary>
    ///  Listen to key collected and lever used 
    /// </summary>
    private void OnEnable()
    {
        Key.OnKeyCollected += TargetKeyCollected;
        Lever.OnLeverUsed+= ChangeToUseLifeboat;
    }

    private void OnDisable()
    {
        Key.OnKeyCollected -= TargetKeyCollected;
        Lever.OnLeverUsed -= ChangeToUseLifeboat;
    }

    /// <summary>
    /// Change the prompt when target key is collected
    /// </summary>
    /// <param name="item"></param>
    public void TargetKeyCollected(ItemData item)
    {
        if(item == unlockCondition)
        {
            prompt = "Use Key to Unlock";
        }
    }

    /// <summary>
    /// Change to use life boat state when lever is finished using
    /// </summary>
    public void ChangeToUseLifeboat()
    {
        curBoatState= LifeboatState.UseLifeboat; // change state 
        prompt = interactionPrompt[curBoatState];// set propmt
        boatUseCollider.enabled= true;
        isLocked = false;
    }

    /// <summary>
    /// Lifeboat interact, handle interactions in different lifeboat condition
    /// </summary>
    /// <param name="interactor"></param>
    /// <returns></returns>
    public bool Interact(Interactor interactor)
    {
        switch(curBoatState)
        {
            case LifeboatState.KeyNeeded:
                
                if (unlockCondition != null)
                {
                    var inventory = interactor.GetComponent<Inventory>();

                    if (inventory == null) return false;
                    if (inventory.inventory != null)
                    {
                        // search for the target key in the inventory
                        if (inventory.SearchForItem(unlockCondition))
                        {
                            curBoatState= LifeboatState.UseLever;                   // Change state
                            inventory.Remove(unlockCondition); // Remove Key
                            prompt = interactionPrompt[curBoatState];               // Reset prompt
                            GameManager.Instance.InteractionUISetUp(prompt, IsDisabled);         // Update UI
                            OnKeyUsedForBoat?.Invoke();                             // Invoke the key is used event
                            return true;
                        }
                        else
                        {
                            prompt = interactionPrompt[curBoatState];
                            return false;
                        }
                    }
                }
                break;
            case LifeboatState.UseLever:
                break;
            case LifeboatState.LowerTheBoat: 
                break;
            case LifeboatState.UseLifeboat:
                GameManager.Instance.ChangeToLifeboatEnding(); // Change to win ending
                break;
        }

       

        return true;
    }
}
