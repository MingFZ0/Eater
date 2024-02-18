using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandDisplay : MonoBehaviour
{
    /* 1. For every card in the hand, display them in an ordered fasion
     * 2. When the mouse clicks and holds on a card, drag the card and move it to mouse position
     * 3. When the mouse lets go of the card, raise an event that is corsponding to the location that it was dropped at.
     */

    [Header("Fields For Displaying the cards in hand")]
    [SerializeField] CardsInHand hand;
    [SerializeReference] private Card selectedCard;

    private bool mouseDown;

    private Collider2D hitbox;

    [Header("Field for the spawnEater Event")]
    [SerializeField] private UnityEvent spawnEater;

    private void Awake() {this.hitbox = gameObject.GetComponent<Collider2D>();}

    public void UpdateHandDisplay()
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
    private void mouseCheck()
    {
        if (!Input.GetMouseButton(0))
        {
            if (this.mouseDown && selectedCard != null)
            {
                this.mouseDown = false;
                this.selectedCard = null;
                UpdateHandDisplay();
            }
            return;

        }
        else { this.mouseDown = true; }
    }
    private void DraggingCard(RaycastHit2D hit, Vector2 movePos) 
    {
        if (selectedCard == null && hit.collider)
        {
            Card card = hit.collider.GetComponent<Card>();
            this.selectedCard = card;
        }
        if (selectedCard) {this.selectedCard.transform.position = movePos;}
    }

    private void Update()
    {   
        //Check for Mouse Activity
        mouseCheck();
           
        //Finds the actual coordinates
        Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);

        RaycastHit2D hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero, 0f);

        if (Input.GetMouseButtonUp(0) && this.selectedCard == true)
        {
            
        }

        //Dragging the selected Card
        DraggingCard(hit, mousePosOnScreen);

    }

}
