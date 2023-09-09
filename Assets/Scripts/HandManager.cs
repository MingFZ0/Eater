using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    private static HandManager _instance;
    public static HandManager Instance { get { return _instance; } }


    public static List<Card> CardsInHand = new List<Card>();
    int recordedCardCount;

    [SerializeField] Collider2D handGUIHitbox;
    [SerializeField] Collider2D deckHitbox;

    [SerializeField] Card cardPrefab;

    bool selectedCard;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);
            //Debug.Log(mousePosInWorld);

            RaycastHit2D hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero, 0f);

            if (hit.collider && hit.transform.gameObject.tag == "Deck")
            {

                if (CardsInHand.Count == 0)
                { 
                    for (int i = 0; i<7; i++)
                    {
                        Card newCard = Instantiate(cardPrefab);
                        CardsInHand.Add(newCard);
                    }
                }
                else if (TurnManager.PhaseCount == 0)
                {
                    Card newCard = Instantiate(cardPrefab);
                    CardsInHand.Add(newCard);
                }

                if (TurnManager.TurnCount <= 0)
                {
                    
                }

                if (TurnManager.PhaseCount == 0)
                {
                    TurnManager.PhaseCount = 1;
                }

            }
        }

        if (CardsInHand.Count != recordedCardCount)
        {
            recordedCardCount = CardsInHand.Count;
            UpdateHandSlots();
        }

        DraggingCards();
          
    }

    private void DraggingCards()
    {
        bool mouseClickHeld = false;

        Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);

        if (Input.GetMouseButton(0))
        {
            mouseClickHeld = true;
        }

        if (Input.GetMouseButtonUp(0) || mouseClickHeld)
        {
            foreach (Card card in CardsInHand)
            {
                if (!mouseClickHeld)
                {
                    card.Selected = false;
                    card.transform.position = card.prevPos;
                    selectedCard = false;
                }
                else if (card.Selected == true) { card.transform.position = mousePosInWorld; }
                
            }
        } else { return; }
        
        if (mouseClickHeld && !selectedCard)
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero);

            if (hit.collider && hit.transform.gameObject.tag == "Card")
            {
                int cardInstanceID = hit.transform.gameObject.GetInstanceID();

                foreach (Card card in CardsInHand)
                {
                    if (card.gameObject.GetInstanceID() == cardInstanceID)
                    {
                        card.Selected = true;
                        selectedCard = true;
                        break;
                    }

                }
            }
        }
        
    }

    private void UpdateHandSlots()
    {
        Debug.Log("Update Hand");
        float hitboxWidth = handGUIHitbox.bounds.size.x;

        int cardCounts = CardsInHand.Count;
        float distanceBetweenEachCard = hitboxWidth / (cardCounts++);

        float currentPos = distanceBetweenEachCard - (hitboxWidth / 2);
        currentPos -= (distanceBetweenEachCard / 2);

        foreach (Card card in HandManager.CardsInHand)
        {
            card.transform.position = new Vector2(currentPos, handGUIHitbox.transform.position.y);
            card.prevPos = new Vector2(currentPos, handGUIHitbox.transform.position.y);
            currentPos += distanceBetweenEachCard;
        }
    }

    public Card RaycastHit2DToObject(RaycastHit2D hitObject, string targetList)
    {
        if (!hitObject.collider || hitObject.transform.gameObject.tag != "Card") { return null; }

        int cardInstanceID = hitObject.transform.gameObject.GetInstanceID();

        //if (hitObject.transform.gameObject.tag == "Card" && hitObject.transform.gameObject != EaterSelected)
        //{
        //    Debug.Log("The eater for this slot has already been selected");
        //    return null;
        //}

        if (targetList == "HandList")
        {
            foreach (Card card in HandManager.CardsInHand)
            {
                if (card.gameObject.GetInstanceID() == cardInstanceID)
                {
                    Debug.Log($"Card Found in HandManager: {card}");
                    return card;
                }
            }
        }

        else if (targetList == "EaterList")
        {
            foreach (Card card in EaterManager.EaterList)
            {
                if (card.gameObject.GetInstanceID() == cardInstanceID)
                {
                    Debug.Log($"Card Found in EaterManager: {card}");
                    return card;
                }
            }
        }

        else if (hitObject.transform.gameObject.tag == "Card")
        {
            return null;
        }

        Debug.LogWarning($"{gameObject.name} Unable to Convert RaycastHit2D of {hitObject.transform.gameObject.name} to Object");
        return null;
    }

}
