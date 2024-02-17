using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaterCard : MonoBehaviour
{
    [SerializeField] EaterList eaterList;

    [Header("Fields for the EaterCard")]
    [SerializeReference] private Sprite CardSprite;
    [SerializeReference] private Sprite CardBackSprite;
    [SerializeReference] private int cardValue;
    [SerializeReference] private CardTypeEnumScriptableObject cardType;

    public void Instantiation(int cardValue, CardTypeEnumScriptableObject cardType)
    {
        this.cardValue = cardValue;
        this.cardType = cardType;
    }

    public int GetCardValue() { return this.cardValue; }
    public CardTypeEnumScriptableObject GetCardType() { return this.cardType; }

    private void OnDisable() { 
        if (eaterList.eaterCount == 1)
        {
            throw new System.Exception("GAME OVER!");
        }
        eaterList.Remove(this); 
    }
    private void OnEnable() { eaterList.Add(this); }

}
