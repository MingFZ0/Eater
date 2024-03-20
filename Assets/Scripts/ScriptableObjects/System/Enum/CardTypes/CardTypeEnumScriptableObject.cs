using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enum/CardTypes")]
public class CardTypeEnumScriptableObject : ScriptableObject
{
    [SerializeField] private string CardType;

    public string getCardType() { return CardType; }

    public static CardTypeEnumScriptableObject stringToCardType(string cardTypeString, List<CardTypeEnumScriptableObject> availableCardType)
    {

        cardTypeString = cardTypeString.ToUpper();
        CardTypeEnumScriptableObject cardType = CreateInstance<CardTypeEnumScriptableObject>();
        cardType.CardType = cardTypeString;
        cardType.name = cardTypeString;
        
        return cardType;
    }
}
