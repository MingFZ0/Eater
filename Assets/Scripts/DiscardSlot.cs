using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardSlot : MonoBehaviour
{
    [SerializeField] CardsInHand hand;
    [SerializeField] GameVariables gameVars;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && hand.selectedCard != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.zero);
            if (hit.collider && hit.collider.gameObject == hand.selectedCard.gameObject)
            {
                Destroy(hand.selectedCard.gameObject);
                hand.UpdateHandDisplay();
            }
        }
    }



}
