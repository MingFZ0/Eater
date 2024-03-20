using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enum/CardTypes")]
public class CardTypeEnumScriptableObject : ScriptableObject
{
    [SerializeField] private string CardType;

    public string getCardType() { return CardType; }
}
