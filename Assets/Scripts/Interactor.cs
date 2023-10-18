using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactRange = 0.5f;
    [SerializeField] private LayerMask interactableMask;

    private readonly Collider[] colliders= new Collider[3];
    [SerializeField] private int numFound;

    private IInteractable interactable;



    private void Update()
    {
        //    Ray r = new Ray(interactionPoint.position, interactionPoint.forward);
        //    if(Physics.Raycast(r, out RaycastHit hitInfo, interactRange, interactableMask))
        //    {
        //        if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactable))
        //        {
        //            interactable.Interact(this);
        //        }
        //    }


        // Check the collide with itneractibles
        // Can also use ray tracing
        numFound = Physics.OverlapSphereNonAlloc(
            interactionPoint.position, 
            interactRange, 
            colliders, 
            interactableMask);

        if(numFound > 0)
        {
            interactable = colliders[0].GetComponent<IInteractable>();
            if(interactable != null)
            {
                // set the interaction UI
                if(!GameManager.Instance.IsDiaplay) 
                {
                    GameManager.Instance.InteractionUISetUp(interactable.InteractionPrompt, interactable.IsDisabled);
                }

                // If the player press e to interact
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    interactable.Interact(this);
                    if (GameManager.Instance.IsDiaplay) GameManager.Instance.CloseInteractionWindow();
                }
            }
        }
        else
        {
            if (interactable != null) { interactable = null; }
            if (GameManager.Instance.IsDiaplay) GameManager.Instance.CloseInteractionWindow();
        }
       
    }

    /// <summary>
    ///  Draw the collide sphere
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionPoint.position, interactRange);
    }
}
