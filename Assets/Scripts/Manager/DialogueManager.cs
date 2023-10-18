using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dialogue Manager
/// Is relate to DialogueTrigger & Dialogue
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialoguePanel; // UI panel for dialogue
    public TMP_Text characterNameText; // Text field for character names
    public TMP_Text dialogueText; // Text field for dialogue text
    public Button continueButton; // Button to progress through dialogue
    

    private Queue<string> sentences; // Queue to hold dialogue lines

    private bool isDialogueActive; // Flag to control dialogue visibility

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        dialoguePanel.SetActive(false);
    }

    void Start()
    {
        sentences = new Queue<string>();
    }

    /// <summary>
    /// Start displaying random sentences from the given dialogue.
    /// Is used by parrot
    /// </summary>
    /// <param name="dialogue">The dialogue to display</param>
    public void StartRandomSentences(Dialogue dialogue)
    {
        // Change the game state to dialogue
        GameManager.Instance.ChangeToDialogue();
        if (!isDialogueActive)
        {
            isDialogueActive = true;
            dialoguePanel.SetActive(true);
            characterNameText.text = dialogue.characterName;
            sentences.Clear();

            sentences.Enqueue(dialogue.sentences[Random.Range(0, dialogue.sentences.Length)]);

            DisplayNextSentence();
        }
    }

    /// <summary>
    /// Start displaying the first sentence from the given dialogue.
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(Dialogue dialogue)
    {
        // Change to Dialogue state
        GameManager.Instance.ChangeToDialogue();

        if (!isDialogueActive)
        {
            isDialogueActive = true;
            dialoguePanel.SetActive(true);
            characterNameText.text = dialogue.characterName;

            sentences.Clear();

            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }
    }

    /// <summary>
    /// Display the next sentence in the dialogue.
    /// </summary>
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    /// <summary>
    /// Type out the given sentence character by character.
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    /// <summary>
    /// End the current dialogue, change the game state, and deactivate the dialogue panel.
    /// </summary>
    public void EndDialogue()
    {
        // Back to game state
        GameManager.Instance.ChangeToGame();

        dialoguePanel.SetActive(false);
        isDialogueActive = false;
    }
}
