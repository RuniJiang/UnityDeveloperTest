using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Only work for Parrot
/// Should be update to a NPC class, and then parrot derived from it
/// </summary>
public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;

    public string InteractionPrompt => prompt;

    [SerializeField] private string npcName;
    [SerializeField] private DialogueTrigger dialogueTrigger;
    //private Dialogue dialogue;

    [SerializeField] private Chest chest;
    private string[] defaultSentences;

    private void OnEnable()
    {
        // Subscribe the code for created then set sentences
        Chest.OnCodeCreated += SetSentences;
    }
    private void OnDisable()
    {
        Chest.OnCodeCreated -= SetSentences;
    }


    public bool IsDisabled => false;

    /// <summary>
    /// Parrot Interaction
    /// </summary>
    /// <param name="interactor"></param>
    /// <returns></returns>
    public bool Interact(Interactor interactor)
    {
        dialogueTrigger.TriggerRandomDialogue();
        return true;
    }

    /// <summary>
    ///  Hard coded set the sentences data
    ///  Will be called after the code is created
    /// </summary>
    public void SetSentences()
    {
        Debug.Log("Create sentences");

        defaultSentences = new string[5]
       {
            "Do you want to know the password?",
            "I don't want to tell you",
            $"The first number is {chest.CodeResult[0]}",
            $"The second number is {chest.CodeResult[1]}",
            $"The third number is {chest.CodeResult[2]}"
       };


        dialogueTrigger = this.GetComponent<DialogueTrigger>();
        dialogueTrigger.dialogue.characterName = npcName;
        dialogueTrigger.dialogue.sentences = defaultSentences;

    }
    
}
