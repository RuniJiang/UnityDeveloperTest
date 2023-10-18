using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Chest with locker
/// </summary>
public class Chest : MonoBehaviour, IInteractable
{

    [SerializeField] private string prompt; // Interaction Prompt
    [SerializeField] private bool isLocked; // If chest is locked, should be locked for this demo

    // IInteractable interaface
    public string InteractionPrompt => prompt;
    public bool IsDisabled => false;

    // Temporary use to show the chest change from locked -> unlocked
    // Should use animation in future
    public GameObject lockedTop;
    public GameObject unlockedTop;

   
  
    private int[] codeResult; // store the correct answer
    public int[] CodeResult { get { return codeResult; } }
    public int[] playerAnswer; // player answer

    // UI for Code Puzzle
    // It should be seperated when there is a UI manager
    [SerializeField] private GameObject codePuzzleWindow; // Entire Code puzzle window
    [SerializeField] private GameObject codePartWindow;   // Three lock code part window
    [SerializeField] private TMP_Text answer1;            // answer text holder
    [SerializeField] private TMP_Text answer2;
    [SerializeField] private TMP_Text answer3;   
    [SerializeField] private GameObject YouFinished;      // Solve puzzle screen

    // Listen to if the code is created
    public static event HandleCodeCreated OnCodeCreated;
    public delegate void HandleCodeCreated();

    private void Awake()
    {
        // hard coded three digit code puzlle
        codeResult = new int[3];
        codeResult[0] = Random.Range(0, 10);
        codeResult[1] = Random.Range(0, 10);
        codeResult[2] = Random.Range(0, 10);
        Debug.Log("CodeReasult:" + codeResult[0] + codeResult[1] + codeResult[2]);

        playerAnswer = new int[3];

        // Set a different initial player answer
        do
        {
            playerAnswer[0] = Random.Range(0, 10);
            playerAnswer[1] = Random.Range(0, 10);
            playerAnswer[2] = Random.Range(0, 10);
        } while (IsPlayerAnswerEqualToCodeResult());  // Make sure the inital value is not same as correct answer

        // Update The puzzle window
        UpdateChestPuzzleWindow();

        Debug.Log("CodeReasult:" + playerAnswer[0] + playerAnswer[1] + playerAnswer[2]);
        OnCodeCreated?.Invoke(); // Fire the code created 
    }

    /// <summary>
    /// Chest interact: Update the chest puzzle window, and open up the UI
    /// </summary>
    /// <param name="interactor">should be player</param>
    /// <returns>interact success</returns>
    public bool Interact(Interactor interactor)
    {
        if(isLocked)
        {
            UpdateChestPuzzleWindow();
            OpenUpChestPuzzle();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Open the Chest Puzzle UI window
    /// </summary>
    private void OpenUpChestPuzzle()
    {
        GameManager.Instance.PauseGame(); // Probably need to consider encapsulate it
        codePuzzleWindow.SetActive(true);
        codePartWindow.SetActive(true);
        YouFinished.SetActive(false);
    }

    /// <summary>
    /// Close the Puzzle window
    /// Is listened by the back button UI
    /// </summary>
    public void CloseChestPuzzle()
    {
        GameManager.Instance.ChangeToGame();
        codePuzzleWindow.SetActive(false);
    }

    /// <summary>
    /// Check if the player answer is same as code
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerAnswerEqualToCodeResult()
    {
        if (playerAnswer.Length != CodeResult.Length)
        {
            Debug.LogError("Code lengths do not match!");
            return false;
        }

        for (int i = 0; i < playerAnswer.Length; i++)
        {
            if (playerAnswer[i] != CodeResult[i])
            {
                return false; // If any digit doesn't match, return false
            }
        }

        return true; // All digits match, return true
    }

    /// <summary>
    /// Increase the digit, listened by each of UI code button
    /// </summary>
    /// <param name="label"></param>
    public void IncreaseAnswer(int label)
    {
        playerAnswer[label] = (playerAnswer[label] + 1) % 10;
        UpdateChestPuzzleWindow();
    }

    /// <summary>
    /// Update the Chest window
    /// </summary>
    private void UpdateChestPuzzleWindow()
    {
        answer1.text = playerAnswer[0].ToString(); // Update the text on the button
        answer2.text = playerAnswer[1].ToString();
        answer3.text = playerAnswer[2].ToString();

        // Check if the result is same
        if(IsPlayerAnswerEqualToCodeResult())  
        {
            isLocked = false;
            this.gameObject.GetComponent<Collider>().enabled = false; // Disable the interaction once is opened
            codePartWindow.SetActive(false);
            YouFinished.SetActive(true);
            unlockedTop.SetActive(true);
            lockedTop.SetActive(false);
        }
    }

    
}
