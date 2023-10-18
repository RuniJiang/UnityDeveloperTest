using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

/// <summary>
/// DialogueTrigger: attached to NPC/talkable objects
/// </summary>
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    /// <summary>
    /// Trigger the dialogue
    /// </summary>
    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }

    /// <summary>
    /// Trigger a random dialogue in the sentences
    /// </summary>
    public void TriggerRandomDialogue()
    {
        DialogueManager.instance.StartRandomSentences(dialogue);
    }
}
