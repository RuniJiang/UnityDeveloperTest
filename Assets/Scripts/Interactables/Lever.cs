using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Lever for the boat
/// </summary>
public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField] private string isLockedPrompt;
    [SerializeField] private string usePrompt;
    [SerializeField] private bool isLocked;
    private string prompt;
    public string InteractionPrompt => prompt;
    public bool IsDisabled => isLocked;


    [SerializeField] private Transform lever; // Reference to the lever
    [SerializeField] private Transform lifeboat; // Reference to the lifeboat
    [SerializeField] private float rotationSpeed = 90.0f; // Rotation speed in degrees per second
    [SerializeField] private float loweringDuration = 5.0f; // Time it takes to lower the lifeboat
    [SerializeField] private float loweringDistance = 10.0f; // Distance to lower the lifeboat
    private bool isLeverActivated = false;


    public static event HandleLeverUsed OnLeverUsed;
    public delegate void HandleLeverUsed();


    private void OnEnable()
    {
        // Listen to if the key is used for the boat
        LifeBoat.OnKeyUsedForBoat += EnableUseLever;
    }

    private void OnDisable()
    {
        LifeBoat.OnKeyUsedForBoat -= EnableUseLever;
    }

    // Enable the lever if the key is used for the boat
    public void EnableUseLever()
    {
        prompt = usePrompt;
        isLocked = false;
    }

    private void Start()
    {
        prompt = isLockedPrompt;
        isLocked = true;
    }

    /// <summary>
    /// Lever interaction
    /// Start to rotate lever if is not locked
    /// </summary>
    /// <param name="interactor"></param>
    /// <returns></returns>
    public bool Interact(Interactor interactor)
    {
        if (isLocked) return false;

        if (!isLeverActivated)
        {
            // Start the lever activation coroutine
            StartCoroutine(ActivateLever());
            
            return true;
        }
        return false;
    }

    /// <summary>
    /// Activates the lever and lowers the lifeboat over time
    /// </summary>
    /// <returns></returns>
    IEnumerator ActivateLever()
    {
        isLeverActivated = true;

        float elapsedTime = 0.0f;
        Vector3 lifeboatStartPosition = lifeboat.position;
        Vector3 lifeboatEndPosition = lifeboatStartPosition - Vector3.up * loweringDistance;

        while (elapsedTime < loweringDuration)
        {
            // Rotate the lever along the X-axis
            lever.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);

            // Lower the lifeboat
            lifeboat.position = Vector3.Lerp(lifeboatStartPosition, lifeboatEndPosition, elapsedTime / loweringDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lifeboat.position = lifeboatEndPosition;

        // Reset the lever
        isLeverActivated = false;
        OnLeverUsed?.Invoke(); // Fire the lever finish lower the boat
        isLocked = true;
        prompt = "Used Already";
    }


}
