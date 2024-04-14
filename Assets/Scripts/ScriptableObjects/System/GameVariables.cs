using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameVariables", menuName = "ScriptableObjects/GameVariables")]
public class GameVariables : ScriptableObject
{
    [Header("<Static Constant Attributes>")]
    [SerializeField] private string END_SCREEN;
    [SerializeField] private int MAX_ROUNDS;
    [SerializeField] private int INITIAL_PRIZE_AMOUNT;
    [SerializeField] private int NUM_OF_EATERS;
    [SerializeField] private int DISCARD_AMOUNT_ALLOWED;
    [SerializeField] private int[] CARD_VALUE_RANGE_EXCLUSIVE;
    [SerializeField] private int HAND_SIZE;
    [SerializeField] private List<CardTypeEnumScriptableObject> availableCardTypes = new List<CardTypeEnumScriptableObject>();

    [SerializeField] private List<GamePhaseEnumSO> availableGamePhase = new List<GamePhaseEnumSO>();
    [Space]
    [Space]

    [Header("<Game Status Attributes> \n<They needs to be reset at the end of every game>")]
    [SerializeReference] private GamePhaseEnumSO gamePhase;
    [SerializeReference] private int gamePhaseIndex;
    [SerializeReference] private int round;
    [SerializeReference] private int turn;
    [SerializeReference] private int score;

    [Header("<Central Game Events>")]
    [SerializeField] private UnityEvent updateStatDisplay;
    [SerializeField] private UnityEvent startOfTurnSetup;
    [SerializeField] private UnityEvent nextRoundSetup;

    [Header("<Runtime Sets>")]
    [SerializeField] private EaterList eaterList;
    [SerializeField] private PrizeCardList prizeList;
    [SerializeField] private CardsInHand hand;
    [SerializeField] private CardsInUse cardsInUse;

    // Public Variables //
    public int Turn { get { return turn; } private set { turn = value; } }
    public int Round { get { return round; } private set { turn = value; } }

    // === Getters and Setters === //
    public int GetTOTAL_ROUNDS() { return MAX_ROUNDS; }
    public int GetINITIALPRIZE() { return INITIAL_PRIZE_AMOUNT; }
    public GamePhaseEnumSO GetGamePhase() { return gamePhase; }
    public int GetNUM_OF_EATERS() { return NUM_OF_EATERS; }
    public int[] GetCARD_VALUE_RANGE() { return CARD_VALUE_RANGE_EXCLUSIVE; }
    public int GetHAND_SIZE() { return HAND_SIZE; }
    public int GetScore() { return score; }
    public int GetDISCARD_AMOUNT_ALLOWED() { return DISCARD_AMOUNT_ALLOWED;}
    public List<CardTypeEnumScriptableObject> GetAvailableCardTypes() { return this.availableCardTypes; }
    public List<GamePhaseEnumSO> GetAvailableGamePhase() { return this.availableGamePhase; }

    // === Scores === //
    public void AddScore() { score++;}
    public void SubtractScore() { score--; }

    // === Phase/Turn/Round Setup and Clean up === //

    //--Turn Setup--//
    /// <summary>
    /// Method that kicks starts the first turn AFTER the eaters get picked at round 0
    /// </summary>
    public void StartFirstTurn()
    {
        Debug.Log("Start turn");
        StartTurnSetup();
        this.gamePhase = availableGamePhase[gamePhaseIndex];
        updateStatDisplay.Invoke();
    }

    /// <summary>
    /// Method that runs at the start of EVERY TURN and resets the necessary fields and attributes for every turn
    /// </summary>
    private void StartTurnSetup()
    {
        startOfTurnSetup.Invoke();
        gamePhaseIndex = 0;
        turn++;
        eaterList.ResetEaterFullCount();
    }

    //--Phase Setup--//
    /// <summary>
    /// Move the game to next phase
    /// </summary>
    public void MoveToNextPhase()
    {
        this.gamePhaseIndex += 1;

        if (gamePhaseIndex >= availableGamePhase.Count || availableGamePhase[gamePhaseIndex].GetDisplayName() == "End Phase") {
            EndPhaseCalculation();
            StartTurnSetup();
        }
        this.gamePhase = availableGamePhase[gamePhaseIndex];

        updateStatDisplay.Invoke();
        //Debug.Log("Current gamePhase is " + gamePhase);
    }
    
    /// <summary>
    /// Does Endphase calculations to check for game over and does the killing of unfull eaters
    /// </summary>
    private void EndPhaseCalculation()
    {
        if (eaterList.GetNumOfEaterFull() == NUM_OF_EATERS) {return;} 
        else
        {
            if (eaterList.GetNumOfEaterFull() == 0)
            {
                SceneManager.LoadScene(END_SCREEN);
            }
            else
            {
                foreach (EaterCard eater in eaterList.GetAllAliveEater())
                {
                    if (eater.GetIsFull() == true) { continue; }
                    eater.spit();
                    score -= eater.GetTotalCardScore();
                    eater.gameObject.SetActive(false);
                }
            }
        }

        //feedingList.Clear();
    }

    //--Round Setup--//
    /// <summary>
    /// Method that gets run at the end of a round
    /// </summary>
    public void EndRound()
    {
        MoveToNextPhase();
        endRoundCleanup();
        nextRoundPrep();
        updateStatDisplay.Invoke();
        eaterList.ResetRoundStats();
        Debug.Log("Round has Ended! You Survived");
    }
    
    /// <summary>
    /// Method that is used to clean up during the end of a round
    /// </summary>
    private void endRoundCleanup()
    {
        //feedingList.Clear();
        hand.Clear();
        hand.selectedCard = null;

        this.gamePhaseIndex = 0;
        this.gamePhase = availableGamePhase[gamePhaseIndex];
        this.turn = 0;
        //if (round < MAX_ROUNDS) { round = MAX_ROUNDS; }
        if (round == MAX_ROUNDS) 
        {
            Debug.Log("You Survived!");
            SceneManager.LoadScene(END_SCREEN);
        } else { round++; }
    }
    
    /// <summary>
    /// Method that is used to prepare for the next round
    /// </summary>
    private void nextRoundPrep()
    {
        nextRoundSetup.Invoke();
        cardsInUse.refill();

        for (int index = 0; index < NUM_OF_EATERS; index++)
        {
            EaterCard eater = eaterList.GetItem(index);
            eater.gameObject.SetActive(true);
            eater.NextRoundSetups();
        }
    }

    // === Data Reset === //
    private void OnValidate()
    {
        resetData();
    }

    public void resetData()
    {
        cardsInUse.resetCards();
        endRoundCleanup();
        eaterList.ResetDataBetweenGames();
        Debug.Log("Game Restart");
        this.gamePhaseIndex = 0;
        this.gamePhase = availableGamePhase[gamePhaseIndex];

        round = 1;
        turn = 0;
        score = 0;
    }
}
