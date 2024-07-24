using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System;

[CreateAssetMenu(fileName = "GameVariables", menuName = "ScriptableObjects/GameVariables")]
public class GameVariables : ScriptableObject
{
    public enum GameState
    {
        ONGOING,
        WON,
        LOSE
    }

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

    [SerializeField] private string gameMode;
    private GamePhaseEnumSO gamePhase;
    private GameState gameState;
    private int gamePhaseIndex;
    private int round;
    private int turn;
    private int score;
    private PopupManager popupManager;

    [Header("<Central Game Events>")]
    [SerializeField] private UnityEvent updateStatDisplay;
    [SerializeField] private UnityEvent startOfTurnSetup;
    [SerializeField] private UnityEvent nextRoundSetup;

    [Header("<Runtime Sets>")]
    [SerializeField] private EaterList eaterList;
    [SerializeField] private PrizeCardList prizeList;
    [SerializeField] private CardsInHand hand;
    [SerializeField] private CardsInUse cardsInUse;

    #region Public Variables
    public int Turn { get { return turn; } private set { turn = value; } }
    public int Round { get { return round; } private set { turn = value; } }
    #endregion

    #region Getters and Setters
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
    #endregion

    #region Scores
    public void AddScore() { score++;}
    public void SubtractScore() { score--; }
    #endregion

    #region Functions: Phase/Turn/Round Setup and Clean up

        #region Turn Setup
    /// <summary>
    /// Method that kicks starts the first turn AFTER the eaters get picked at round 0
    /// </summary>
    public void StartFirstTurn()
    {
        gameState = GameState.ONGOING;
        Debug.Log("Start turn");
        
        StartTurnSetup();
        this.gamePhase = availableGamePhase[gamePhaseIndex];
        updateStatDisplay.Invoke();
        //popupManager.NotifyDrawPhase(2);
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
        //popupManager.NotifyDrawPhase(2);
    }

    #endregion

        #region Phase Setup
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
        //else if (availableGamePhase[gamePhaseIndex].GetDisplayName() == "Action Phase") { popupManager.NotifyActionPhase(2); }
        //else if (availableGamePhase[gamePhaseIndex].GetDisplayName() == "Draw Phase") { popupManager.NotifyDrawPhase(2); }
        Debug.Log("Called");
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
                gameState = GameState.LOSE;
                saveData();
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

    #endregion

        #region Round Setup
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
            gameState = GameState.WON;
            saveData();
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

    #endregion

        #region Data Reset or Saving
    private void OnValidate()
    {
        resetData();
    }

    public void resetData()
    {
        if (popupManager != null) { Destroy(popupManager); }
        popupManager = null;
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

    public void saveData()
    {
        ModeData modeData = new ModeData(gameMode, gameState.ToString(), score, round, eaterList.GetNumOfEaterFull());

        if (File.Exists(Application.dataPath + "/SaveData.save") && JsonUtility.FromJson<SavedData>(File.ReadAllText(Application.dataPath + "/SaveData.save")) != null) 
        { 
            updateSaved(modeData); 
        }
        else {
            SavedData savedData = new SavedData();
            savedData.modesData.Add(modeData);
            string json = JsonUtility.ToJson(savedData);
            Debug.Log(json);
            Debug.Log(Application.dataPath);
            File.WriteAllText(Application.dataPath + "/SaveData.save", json); 
        }
    }

    private void updateSaved(ModeData currentData)
    {
        string fileContent = File.ReadAllText(Application.dataPath + "/SaveData.save");
        SavedData savedData = JsonUtility.FromJson<SavedData>(fileContent);

        if (savedData.ContainsMode(gameMode))
        {
            ModeData data = savedData.GetModeData(gameMode);
            if (data.score >= currentData.score)
            {
                Debug.Log("New score!");
                savedData.modesData.Remove(data);
                savedData.modesData.Add(currentData);
            } else {
                Debug.Log("You had a better score than this"); 
            }
        }
        else
        {
            savedData.modesData.Add(currentData);
        }

        string json = JsonUtility.ToJson(savedData);
        File.WriteAllText(Application.dataPath + "/SaveData.save", json);
    }
        #endregion
    
    #endregion
}

[Serializable]
public class SavedData
{
    public List<ModeData> modesData = new List<ModeData>();

    public bool ContainsMode(string gameMode)
    {
        foreach (ModeData modeData in modesData)
        {
            if (modeData.gameMode.Equals(gameMode)) { return true; }
        }
        return false;
    }

    public ModeData GetModeData(string gameMode)
    {
        foreach (ModeData modeData in modesData)
        {
            if (modeData.gameMode.Equals(gameMode)) { return modeData; }
        }
        return null;
    }
}

[Serializable]
public class ModeData {
    public string gameMode;
    public string gameState;
    public int score;
    public int survivedRound;
    public int remainingEater;

    public ModeData(string mode, string gameState, int score, int survivedRound, int remainingEater)
    {
        this.gameMode = mode;
        this.gameState = gameState;
        this.score = score;
        this.survivedRound = survivedRound;
        this.remainingEater = remainingEater;
    }
}
