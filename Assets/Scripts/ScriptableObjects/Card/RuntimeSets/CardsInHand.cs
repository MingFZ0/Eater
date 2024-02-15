using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* One limitation of this technique is that if you inspect the ScriptableObject at runtime, you won’t be able to see the 
 * contents of the Items list in the Inspector by default. Instead, a “Type mismatch” appears in each element field. 
 * By design, a ScriptableObject can’t serialize a scene object. The list is actually working, but the data does not display correctly.
 */

[CreateAssetMenu(fileName = "CardsInHand", menuName = "ScriptableObjects/RuntimeSet/CardsInHand")]

public class CardsInHand : RuntimeSetSO<Card>
{

    [SerializeField]
    public int handLength 
    { get { return items.Count; } private set { } }

    public override void Add(Card card)
    {
        if (!items.Contains(card) && !ContainSameValueInList(card))
        {
            items.Add(card);
            Debug.Log("card added");
        } else { throw new System.Exception(card + " is already found in list. Unable to add it"); }
    }

    public override Card GetValue(int index)
    {
        return items[index];
    }

    public override void Remove(Card card)
    {
        if (items.Contains(card)) { items.Remove(card); }
        else { throw new System.Exception("Unable to find " + card + " in list"); }
    }

    public int GetHandLength() { return items.Count; }

    public bool ContainSameValueInList(Card card)
    {
        int value = card.GetCardValue();
        CardTypeEnumScriptableObject type = card.GetCardType();

        for (int i = 0; i < items.Count; i++)
        {
            if (value == items[i].GetCardValue() && type == items[i].GetCardType()) {return true;}
        }

        return false;
    }

    public bool ContainSameValueInList(int value, CardTypeEnumScriptableObject type)
    {

        for (int i = 0; i < items.Count; i++)
        {
            if (value == items[i].GetCardValue() && type == items[i].GetCardType()) { return true; }
        }

        return false;
    }
}
