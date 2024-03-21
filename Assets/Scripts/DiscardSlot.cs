using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardSlot : MonoBehaviour
{
    [SerializeField] CardsInHand hand;
    [SerializeField] CardsInUse cardsInUse;
    [SerializeField] GameVariables gameVars;
    [SerializeReference] int discardAmount;

    [SerializeField] TextMesh displayText;

    private void Awake()
    {
        discardAmount = gameVars.GetDISCARD_AMOUNT_ALLOWED();
        displayText.text = discardAmount.ToString();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && hand.selectedCard != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.zero);
            if (hit.collider && hit.collider.gameObject == hand.selectedCard.gameObject && discardAmount > 0)
            {
                discardAmount--;
                displayText.text = discardAmount.ToString();
                cardsInUse.addToUnavailable(hand.selectedCard);
                Destroy(hand.selectedCard.gameObject);
                hand.UpdateHandDisplay();
            }
        }
    }

    public void StartOfTurnSetup() 
    { 
        discardAmount = gameVars.GetDISCARD_AMOUNT_ALLOWED(); 
        displayText.text = discardAmount.ToString();
    }



}
