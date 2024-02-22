using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaterCard : MonoBehaviour
{
    [SerializeField] EaterList eaterList;
    [SerializeField] CardsInHand hand;

    [Header("Fields for the EaterCard")]
    [SerializeReference] private Sprite CardSprite;
    [SerializeReference] private Sprite CardBackSprite;
    [SerializeReference] private int cardValue;
    [SerializeReference] private CardTypeEnumScriptableObject cardType;
    [SerializeReference] private bool isInstantiated;

    public void Instantiation(int cardValue, CardTypeEnumScriptableObject cardType)
    {
        this.cardValue = cardValue;
        this.cardType = cardType;
        this.name = "EATER " + cardType.name + " of " + cardValue;
        this.isInstantiated = true;
    }


    public int GetCardValue() { return this.cardValue; }
    public CardTypeEnumScriptableObject GetCardType() { return this.cardType; }

    
    public void TrySpawnEater()
    {
        if (this.isInstantiated) { return; }
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.zero);
        if (hit.collider && hand.selectedCard != null)
        {
            if (hit.collider.gameObject != hand.selectedCard.gameObject) { return; }
            Card card = hand.selectedCard;
            Instantiation(card.GetCardValue(), card.GetCardType());
            hand.Destory(card);
        }
    }


    private void OnDisable() { 
        if (eaterList.EaterCount == 1)
        {
            throw new System.Exception("GAME OVER!");
        }
        eaterList.Remove(this); 
    }
    private void OnEnable() { eaterList.Add(this); }

}
