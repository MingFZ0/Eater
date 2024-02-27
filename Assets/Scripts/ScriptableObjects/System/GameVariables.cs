using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameVariables", menuName = "ScriptableObjects/GameVariables")]
public class GameVariables : ScriptableObject
{
    [Header("<Static Constant Attributes>")]
    [SerializeField] private int MAX_ROUNDS;
    [SerializeField] private int INITIAL_PRIZE_AMOUNT;
    [SerializeField] private int NUM_OF_EATERS;
    [SerializeField] private int DISCARD_AMOUNT_ALLOWED;
    [SerializeField] private int[] CARD_VALUE_RANGE_EXCLUSIVE = { 1, 13 };
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
    [SerializeField] private EaterList feedingList;
    [SerializeField] private PrizeCardList prizeList;
    [SerializeField] private CardsInHand hand;


    public int Turn { get { return turn; } private set { turn = value; } }
    public int Round { get { return round; } private set { turn = value; } }

    private void OnValidate()
    {
        this.gamePhaseIndex = 0;
        this.gamePhase = availableGamePhase[gamePhaseIndex];

        round = 0;
        turn = 0;
        score = 0;
    }

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

    public void AddScore() { score++;}
    public void SubtractScore() { score--; }

    public void StartFirstTurn()
    {
        StartTurnSetup();
        this.gamePhase = availableGamePhase[gamePhaseIndex];
        updateStatDisplay.Invoke();
    }
    private void StartTurnSetup()
    {
        for (int i = 0; i < eaterList.EaterCount; i++)
        {
            EaterCard eater = eaterList.GetItem(i);
            feedingList.Add(eater);
        }

        startOfTurnSetup.Invoke();
        gamePhaseIndex = 0;
        turn++;
    }

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

    private void EndPhaseCalculation()
    {
        if (feedingList.EaterCount == 0)
        {
            return;
        } else
        {
            if (feedingList.EaterCount == eaterList.EaterCount)
            {
                Application.Quit();
                throw new System.Exception("GAME OVER!");
            }
            else
            {
                for (int i = 0; i < feedingList.EaterCount; i++)
                {
                    EaterCard eater = feedingList.GetItem(i);
                    eater.spit();
                    score -= eater.GetTotalCardScore();
                    eaterList.Remove(eater);
                    eater.gameObject.SetActive(false);
                }
            }
        }

        feedingList.Clear();
    }


    private void endRoundCleanup()
    {
        feedingList.Clear();
        hand.Clear();
        hand.selectedCard = null;

        this.gamePhaseIndex = 0;
        this.gamePhase = availableGamePhase[gamePhaseIndex];
        this.turn = 0;
        round++;
    }
    public void EndRound()
    {

        nextRoundSetup.Invoke();
        endRoundCleanup();
        updateStatDisplay.Invoke();
        Debug.Log("Round has Ended! You Survived");
    }
}
