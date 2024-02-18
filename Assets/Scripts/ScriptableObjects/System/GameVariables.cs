using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameVariables", menuName = "ScriptableObjects/GameVariables")]
public class GameVariables : ScriptableObject
{
    [SerializeReference] private GamePhaseEnum gamePhase;

    [SerializeField] private int NUM_OF_EATERS;

    public GamePhaseEnum GetGamePhase() { return gamePhase; }
    public int GetNUM_OF_EATERS() { return NUM_OF_EATERS; }
    
}
