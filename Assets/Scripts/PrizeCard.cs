using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeCard : MonoBehaviour
{

    [Header("Fields for the Card")]
    [SerializeField] CardsInHand hand;

    [Header("Fields for the PrizeCard")]
    [SerializeReference] private Sprite CardSprite;
    [SerializeReference] private Sprite CardBackSprite;
    [SerializeReference] private int cardValue;
    [SerializeReference] private CardTypeEnumScriptableObject cardType;

    /* === [Fields for PrizeCard Display] */
    [Header("Attributes Used for Card Display")]
    [SerializeReference] private bool isRevealed;

    /* === [Fields for GameEvents For PrizeCards] */
    [Header("Fields for GameEvents For PrizeCards")]
    [SerializeField] private GameEvent PrizeCardClicked;          //When you click on a facedown prizecard, you reveal it
                                                                  //When you click on a revealed prizecard, you draw it


    public void Instantiation(int cardValue, CardTypeEnumScriptableObject cardType)
    {
        this.cardValue = cardValue;
        this.cardType = cardType;
        name = "[PrizeCard] " + cardValue + " of " + cardType.ToString();
        this.isRevealed = false;

    }


}
