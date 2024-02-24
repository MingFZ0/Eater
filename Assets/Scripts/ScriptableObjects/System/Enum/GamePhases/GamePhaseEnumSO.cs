using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enum/GamePhaseType")]
public class GamePhaseEnumSO : ScriptableObject
{
    [SerializeField] string displayName;

    public string GetDisplayName() { return displayName; }


}
