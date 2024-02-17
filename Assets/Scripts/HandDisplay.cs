using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDisplay : MonoBehaviour
{
    /* 1. For every card in the hand, display them in an ordered fasion
     * 2. When the mouse clicks and holds on a card, drag the card and move it to mouse position
     * 3. When the mouse lets go of the card, raise an event that is corsponding to the location that it was dropped at.
     */

    [SerializeField] CardsInHand hand;
    [SerializeReference] private Card selectedCard;

    private Collider2D hitbox;

    private void Awake()
    {
        this.hitbox = gameObject.GetComponent<Collider2D>();
    }

    public void updateHandDisplay()
    {
        int handSize = hand.GetHandLength();
        Debug.Log("Hand Size is " + handSize);
        float hitboxWidth = hitbox.bounds.size.x;
        float distanceBetweenEachCard = hitboxWidth / (handSize);

        float currentIteratingPos = distanceBetweenEachCard - (hitboxWidth / 2);
        currentIteratingPos -= (distanceBetweenEachCard / 2);

        for (int i = 0; i < handSize; i++)
        {
            Card card = hand.GetItem(i);
            card.transform.position = new Vector2(currentIteratingPos, hitbox.transform.position.y);
            card.previousPos = new Vector2(currentIteratingPos, hitbox.transform.position.y);
            currentIteratingPos += distanceBetweenEachCard;
        }
    }

}
