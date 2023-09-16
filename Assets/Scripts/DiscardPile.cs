using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    Collider2D hitBox;

    public static int DiscardCount;

    private void Awake()
    {
        hitBox = gameObject.GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && TurnManager.TurnCount > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, Vector2.zero);

            if (hit.collider && hit.transform.gameObject.tag == "Card" && DiscardCount < 2 && TurnManager.TurnCount >= 1 && EaterManager.StoredList.Count == 0)
            {

                int currentCardInstanceId = hit.transform.gameObject.GetInstanceID();

                foreach (Card card in HandManager.CardsInHand)
                {

                    if (currentCardInstanceId == card.gameObject.GetInstanceID())
                    {
                        Debug.Log($"Discarded {card.name}");
                        Destroy(card.gameObject); //if you just want to hide it, do card instead of card.gameobject
                        DiscardCount++;
                        break;
                    }
                }
            }
        }
    }
}
