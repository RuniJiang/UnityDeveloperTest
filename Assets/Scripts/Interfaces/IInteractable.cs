using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;


public interface IInteractable
{
    /// <summary>
    /// text prompt for interacting with the object
    /// </summary>
    public string InteractionPrompt { get; }

    /// <summary>
    /// Check if the interction is disable
    /// For example, for a locked door, the player needs to have a key to interact
    /// It is mostly used for lock icon when the interaction is disabled
    /// </summary>
    public bool IsDisabled { get; }

    
    /// <summary>
    /// Interact methods with IInteractable, used with interactor
    /// </summary>
    /// <param name="interactor">The interactor class object</param>
    /// <returns>If the interaction is success</returns>
    public bool Interact(Interactor interactor);
}
