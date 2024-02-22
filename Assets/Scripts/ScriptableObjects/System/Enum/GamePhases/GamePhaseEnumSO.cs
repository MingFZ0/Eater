using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enum/GamePhaseType")]
public class GamePhaseEnumSO : ScriptableObject
{
    public string GamePhase { get; private set; }
}
