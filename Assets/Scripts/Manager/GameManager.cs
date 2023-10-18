using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        Game,
        Dialogue,
        GameOver,
        YouWin
    }

    #region Singleton
    private static GameManager instance;

    public static GameManager Instance
    {
        get 
        {
            if (instance == null)
                Debug.LogError("GameManager is NULL");

            return instance; 
        }
    }


    private void Awake()
    {
        if(instance)
            Destroy(gameObject);
        else
            instance= this;

        DontDestroyOnLoad(this);
    }
    #endregion

    [SerializeField] private GameObject Player;

    private DialogueManager dialogueManager;

    private GameState curGameState;

    // Key and Lifeboat
    public GameObject keyForLifeBoat;
    [SerializeField] private Collider keyArea1;

    // Timer 
    [SerializeField] private float TimeLeft;
    [SerializeField] private bool TimerOn = false;
    [SerializeField] private TMP_Text TimerText;

    // UI for interaction propmt
    [SerializeField] private TMP_Text interactionPropmtText;
    [SerializeField] private GameObject interactionWindow;
    [SerializeField] private GameObject eKeySprite;
    [SerializeField] private GameObject lockedSprite;
    public bool IsDiaplay = false;

    // UI for inventory
    [SerializeField] private TMP_Text inventoryText;
    [SerializeField] private Inventory inventory;

    // UI for Win & Lose
    [SerializeField] private GameObject GameEndWindow;
    [SerializeField] private TMP_Text EndPhrase;
    [SerializeField] private TMP_Text Explaination;

    // Use steerwheel Ending and Related UI
    [SerializeField] private SteeringWheel SteeringWheel;
    [SerializeField] private GameObject gunUseChoiceWindow;
    [SerializeField] private Button useGun;
    [SerializeField] private Button notUseGun;

    // Ending explaination
    [TextArea(3, 10)]
    [SerializeField] private string timeOutEnding;
    [TextArea(3, 10)]
    [SerializeField] private string forcellyControl;
    [TextArea(3, 10)]
    [SerializeField] private string killCaptain;
    [TextArea(3, 10)]
    [SerializeField] private string notKillCaptain;
    [TextArea(3, 10)]
    [SerializeField] private string escapeThroughLifeboat;


    // Start is called before the first frame update
    void Start()
    {
        dialogueManager= GetComponent<DialogueManager>();

        curGameState= GameState.Game;

        TimerOn = true;
        PlaceKeyRandomly();

        interactionWindow.SetActive(false);
        GameEndWindow.SetActive(false);
        gunUseChoiceWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       // Check the curGameState
        switch (curGameState)
        {
            // Game, Handle Timer, SteerWheel
            case GameState.Game:
                if(TimerOn)
                {
                    if(TimeLeft > 0)
                    {
                        TimeLeft -= Time.deltaTime;
                     
                    }
                    else
                    {
                        TimeLeft= 0;
                        TimerOn= false;
                        ChangeToLose(timeOutEnding);
                    }
                    UpdateTimer(TimeLeft);
                }

                // Check if the steerwheel is used
                // Maybe it should change to a delagate
                if(SteeringWheel.IsForceUsed)
                {
                    ChangeToLose(forcellyControl);
                    PauseGame();
                }
                if(SteeringWheel.IsUsedWithGun)
                {
                    gunUseChoiceWindow.SetActive(true);
                    PauseGame();
                }
                break;
            case GameState.Dialogue:
                break;

            case GameState.GameOver:
                break;
                    
        }
    }

    /// <summary>
    /// Upause Game
    /// </summary>
    private void UnpauseGame()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Player.SetActive(true);
    }

    /// <summary>
    /// Pause the game for dialogue, Puzzle
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Player.SetActive(false);
    }

    /// <summary>
    /// Place the key in a random place
    /// </summary>
    private void PlaceKeyRandomly()
    {
        Bounds bounds = keyArea1.bounds;

        // Generate random number
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        // Set the key's position
        keyForLifeBoat.transform.position = new Vector3(randomX, randomY, randomZ);

        Debug.Log("Key position: " + randomX + " " + randomY + " " + randomZ);
    }

 
    /// <summary>
    /// Update Timer UI
    /// </summary>
    /// <param name="curTime"></param>
    private void UpdateTimer(float curTime)
    {
        float minutes = Mathf.FloorToInt(curTime / 60);
        float seconds = Mathf.FloorToInt(curTime % 60);
        TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    /// <summary>
    /// Set up Interaction UI window
    /// </summary>
    /// <param name="propmtText">Interaction Prompt to display</param>
    /// <param name="isLocked">Check if the interactable is locked</param>
    public void InteractionUISetUp(string propmtText, bool isLocked)
    {
        interactionPropmtText.text = propmtText;
        interactionWindow.SetActive(true);
   
        // Change the UI to corresponding sprite: lock or E key
        if (isLocked)
        {
            eKeySprite.SetActive(false);
            lockedSprite.SetActive(true);
        }
        else
        {
            eKeySprite.SetActive(true);
            lockedSprite.SetActive(false);
        }
        IsDiaplay = true;

    }

    /// <summary>
    /// Close the InteractionUIWindow
    /// </summary>
    public void CloseInteractionWindow()
    {
        interactionWindow.SetActive(false);
        IsDiaplay = false;
    }

    /// <summary>
    /// Show the Inventory item on the UI
    /// </summary>
    public void ShowItemInInventory()
    {
        inventoryText.text = "";
        foreach (InventoryItem item in inventory.inventory)
        {
            inventoryText.text += $"{item.itemData.displayName} x {item.stackSize} \n";
        }
    }

    /// <summary>
    /// Set the ending UI window and change the game state
    /// </summary>
    /// <param name="explaination">Explaination of the ending</param>
    public void ChangeToWin(string explaination)
    {
        curGameState = GameState.YouWin;
        GameEndWindow.SetActive(true);
        EndPhrase.text = "You Win";
        Explaination.text = explaination;
        PauseGame();
    }

    /// <summary>
    /// Set the ending UI window and change the game state
    /// </summary>
    /// <param name="explaination"></param>
    public void ChangeToLose(string explaination)
    {
        curGameState = GameState.GameOver;
        gunUseChoiceWindow.SetActive(false);
        GameEndWindow.SetActive(true);
        EndPhrase.text = "Game Over";
        Explaination.text = explaination;
        PauseGame();
    }

    /// <summary>
    /// Change to Life boat Ending
    /// </summary>
    public void ChangeToLifeboatEnding()
    {
        ChangeToWin(escapeThroughLifeboat);
    }

 
    /// <summary>
    /// Not use gun ending
    /// Subscribed by the Not Use Gun Button
    /// </summary>
    public void ChangeToNotUseGunEnding()
    {
        ChangeToLose(notKillCaptain);
    }

    // <summary>
    /// use gun ending
    /// Subscribed by the Use Gun Button
    /// </summary>
    public void ChangeToUseGunEnding()
    {
        ChangeToLose(killCaptain);
    }

    /// <summary>
    /// Set back to game state
    /// </summary>
    public void ChangeToGame()
    {
        UnpauseGame();
        curGameState = GameState.Game;
    }

    /// <summary>
    /// Set to Dialogue state
    /// </summary>
    public void ChangeToDialogue()
    {
        IsDiaplay = false;
        interactionWindow.SetActive(IsDiaplay);
        PauseGame();
        curGameState = GameState.Dialogue;
    }
}
