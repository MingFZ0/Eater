using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameVariables", menuName = "ScriptableObjects/GameVariables")]
public class GameVariables : ScriptableObject
{
    [Header("<Static Constant Attributes>")]
    [SerializeField] private int NUM_OF_EATERS;
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

    [Header("<Central Game Events>")]
    [SerializeField] private UnityEvent updateStatDisplay;
    [SerializeField] private UnityEvent startOfTurnSetup;

    [Header("<Runtime Sets>")]
    [SerializeField] private EaterList eaterList;
    [SerializeField] private EaterList feedingList;
    [SerializeField] private CardsInHand hand;

    public int Turn { get { return turn; } private set { turn = value; } }
    public int Round { get { return round; } private set { turn = value; } }

    private void Awake()
    {
        this.gamePhaseIndex = 0;
        this.gamePhase = availableGamePhase[gamePhaseIndex];
    }


    public GamePhaseEnumSO GetGamePhase() { return gamePhase; }
    public int GetNUM_OF_EATERS() { return NUM_OF_EATERS; }
    public int[] GetCARD_VALUE_RANGE() { return CARD_VALUE_RANGE_EXCLUSIVE; }
    public int GetHAND_SIZE() { return HAND_SIZE; }
    public List<CardTypeEnumScriptableObject> GetAvailableCardTypes() { return this.availableCardTypes; }
    public List<GamePhaseEnumSO> GetAvailableGamePhase() { return this.availableGamePhase; }


    public void StartFirstTurn()
    {
        StartTurnSetup();
        this.gamePhase = availableGamePhase[gamePhaseIndex];
        updateStatDisplay.Invoke();
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
                    eaterList.Remove(eater);
                    Destroy(eater.gameObject);
                }
            }
        }

        feedingList.Clear();
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

    private void OnDisable()
    {
        gamePhaseIndex = 0;
        gamePhase = availableGamePhase[gamePhaseIndex];

        round = 0;
        turn = 0;

    }
}
