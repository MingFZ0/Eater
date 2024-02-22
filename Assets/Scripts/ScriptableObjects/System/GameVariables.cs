using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameVariables", menuName = "ScriptableObjects/GameVariables")]
public class GameVariables : ScriptableObject
{
    [Header("Static Constant Attributes")]
    [SerializeField] private int NUM_OF_EATERS;
    [SerializeField] private int[] CARD_VALUE_RANGE_EXCLUSIVE = { 1, 13 };
    [SerializeField] private List<CardTypeEnumScriptableObject> availableCardTypes = new List<CardTypeEnumScriptableObject>();

    [SerializeField] private List<GamePhaseEnumSO> availableGamePhase = new List<GamePhaseEnumSO>();
    [SerializeReference] private GamePhaseEnumSO gamePhase;
    [SerializeReference] private int gamePhaseIndex;


    public int Turn { get { return Turn; } private set { } }
    public int Round { get { return Round; } private set { } }

    private void Awake()
    {
        this.gamePhaseIndex = 0;
        this.gamePhase = availableGamePhase[gamePhaseIndex];
    }


    public GamePhaseEnumSO GetGamePhase() { return gamePhase; }
    public int GetNUM_OF_EATERS() { return NUM_OF_EATERS; }
    public int[] GetCARD_VALUE_RANGE() { return CARD_VALUE_RANGE_EXCLUSIVE; }
    public List<CardTypeEnumScriptableObject> GetAvailableCardTypes() { return this.availableCardTypes; }
    public List<GamePhaseEnumSO> GetAvailableGamePhase() { return this.availableGamePhase; }


    public void MoveToNextPhase()
    {
        this.gamePhaseIndex += 1;

        if (gamePhaseIndex >= availableGamePhase.Count) { gamePhaseIndex = 0; }
        this.gamePhase = availableGamePhase[gamePhaseIndex];

        Debug.Log("Current gamePhase is " + gamePhase);
    }

}
