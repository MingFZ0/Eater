using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* One limitation of this technique is that if you inspect the ScriptableObject at runtime, you won’t be able to see the 
 * contents of the Items list in the Inspector by default. Instead, a “Type mismatch” appears in each element field. 
 * By design, a ScriptableObject can’t serialize a scene object. The list is actually working, but the data does not display correctly.
 */

[CreateAssetMenu(fileName = "EaterList", menuName = "ScriptableObjects/RuntimeSet/EaterList")]

public class EaterList : RuntimeSetSO<EaterCard>
{
    [SerializeField] private int NUM_OF_EATERS;

    [SerializeField]
    public int EaterCount
    { get { return items.Count; } private set { } }


    public override void Add(EaterCard eaterCard)
    {
        if (!items.Contains(eaterCard) && items.Count < NUM_OF_EATERS) { items.Add(eaterCard); }
        else if (!items.Contains(eaterCard) && items.Count > NUM_OF_EATERS) { throw new System.Exception("Too many Eaters"); }
    }

    public override EaterCard GetItem(int index)
    {
        return items[index];
    }

    public override void Remove(EaterCard eaterCard)
    {
        items.Remove(eaterCard);
    }
}
