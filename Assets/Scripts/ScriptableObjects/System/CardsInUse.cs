using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardsInUse", menuName = "ScriptableObjects/CardsInUse")]
public class CardsInUse : ScriptableObject
{
    [SerializeField] private Sprite[] spadeSet;
    [SerializeField] private Sprite[] cloverSet;
    [SerializeField] private Sprite[] diamondSet;
    [SerializeField] private Sprite[] heartSet;

    public Sprite getCardSprite(CardTypeEnumScriptableObject cardType, int cardValue)
    {
        if (cardType.getCardType() == "SPADE") { return spadeSet[cardValue-1]; }
        if (cardType.getCardType() == "CLOVER") { return cloverSet[cardValue-1]; }
        if (cardType.getCardType() == "DIAMOND") { return diamondSet[cardValue-1]; }
        if (cardType.getCardType() == "HEART") { return heartSet[cardValue-1]; }
        if (cardType.getCardType() == "JOKER") { return heartSet[cardValue - 1]; }
        else { throw new System.Exception("Unknown card type"); }
    }

}
