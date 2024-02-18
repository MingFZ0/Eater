using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameVariables", menuName = "ScriptableObjects/GameVariables")]
public class GameVariables : ScriptableObject
{
    [SerializeReference] private GamePhaseEnum gamePhase;

    [SerializeField] private int NUM_OF_EATERS;

    [SerializeField] private int[] CARD_VALUE_RANGE_EXCLUSIVE = { 1, 13 };

    public GamePhaseEnum GetGamePhase() { return gamePhase; }
    public int GetNUM_OF_EATERS() { return NUM_OF_EATERS; }

    public int[] GetCARD_VALUE_RANGE() { return CARD_VALUE_RANGE_EXCLUSIVE; }
    
}
