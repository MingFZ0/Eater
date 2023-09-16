using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaterSlots : MonoBehaviour
{
    SpriteRenderer spriteRender;
    BoxCollider2D hitboxCollider;

    private Card EaterSelected;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        hitboxCollider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FirstTurnCheck();


        // Feeding Mechanic
        if (TurnManager.TurnCount >= 1 && TurnManager.PhaseCount >= 1 && Input.GetMouseButtonUp(0))
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(EaterSelected.transform.position, Vector2.zero);
                
            foreach (RaycastHit2D hitObject in hit)
            {
                if (!hitObject.collider) { continue; }
                float hitObjectID = hitObject.transform.GetInstanceID();

                if (hitObjectID == EaterSelected.transform.GetInstanceID()) { continue; }
                if (hitObject.transform.tag == "Card")
                {
                    foreach (Card cardInHand in HandManager.CardsInHand)
                    {

                        if (cardInHand.transform.GetInstanceID() == hitObjectID)
                        {
                            if (EaterManager.FullList.Contains(EaterSelected)) { return; }
                            EaterManager.CurrentEater = EaterSelected;
                            Card feedingCard = cardInHand;


                            EaterManager.StoredList.Add(feedingCard);

                            feedingCard.stored = true;
                            feedingCard.gameObject.SetActive(false);

                            Debug.Log(EaterManager.StoredList.Count);

                            EaterManager.CurrentHunger -= feedingCard.CardValue;

                            //Debug.Log($"EaterSelected is {EaterSelected.name}");


                            //EaterManager.ValueLeftToEat[eaterSlotNum] -= feedingCard.CardValue;
                            

                            //Destroy(feedingCard.gameObject);

                        }
                    }
                }
            }
        }

    }

    private Card RaycastHit2DToObject(RaycastHit2D hitObject, string targetList)
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
            foreach (Card card in HandManager.CardsInHand) {if (card.gameObject.GetInstanceID() == cardInstanceID) 
                {
                    //Debug.Log($"Card Found in HandManager: {card}");
                    return card;
                }}
        }

        else if (targetList == "EaterList")
        {
            foreach (Card card in EaterManager.EaterList) {if (card.gameObject.GetInstanceID() == cardInstanceID) 
                {
                    //Debug.Log($"Card Found in EaterManager: {card}");
                    return card;
                }}
        }

        else if (hitObject.transform.gameObject.tag == "Card")
        {
            return null;
        }

        Debug.LogWarning($"{gameObject.name} Unable to Convert RaycastHit2D of {hitObject.transform.gameObject.name} to Object");
        return null;
    }

 
    void FirstTurnCheck()
    {
        if (TurnManager.TurnCount > 0) { return; }

        if (Input.GetMouseButtonUp(0) && !EaterSelected)      //Detect card being moved to EaterSlot
        {
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, Vector2.zero);

            Card selectedCard = RaycastHit2DToObject(hit, "HandList");

            if (selectedCard == null) { return; }

            selectedCard.Eater = true;
            selectedCard.Selected = false;
            selectedCard.transform.position = new Vector3(spriteRender.transform.position.x, spriteRender.transform.position.y, 10);
            HandManager.CardsInHand.Remove(selectedCard);
            EaterManager.EaterList.Add(selectedCard);
            
            //EaterManager.ValueLeftToEat[eaterSlotNum] = selectedCard.CardValue;
            EaterSelected = selectedCard;

            if (EaterManager.EaterList.Count == 2)
            {
                TurnManager.PhaseCount = 2;
            }
        }

        else if (Input.GetMouseButton(0) && EaterSelected)      //Detect card being moved out from EaterSlot
        {
            Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);

            RaycastHit2D hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero);

            try
            {
                Card selectedCard = RaycastHit2DToObject(hit, "EaterList");

                if (selectedCard == null || selectedCard != EaterSelected) { return; }

                selectedCard.Eater = false;
                EaterManager.EaterList.Remove(selectedCard);
                //EaterManager.ValueLeftToEat[eaterSlotNum] = 0;
                HandManager.CardsInHand.Add(selectedCard);

                EaterSelected = null;

                selectedCard.transform.position = selectedCard.prevPos;

                if (EaterManager.EaterList.Count < 2)
                {
                    TurnManager.PhaseCount = 1;
                }
            }
            catch
            {
                return;
            }
            
        }
    }

}
