using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enum/CardTypes")]
public class CardTypeEnumScriptableObject : ScriptableObject
{
    public string CardType { get; private set; }
}
