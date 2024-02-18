using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrizeCardList", menuName = "ScriptableObjects/RuntimeSet/PrizeCardList")]
public class PrizeCardList : RuntimeSetSO<EaterCard>
{
    public int PrizeCardsCount
    { get { return items.Count; } private set { } }

    public override void Add(EaterCard thing)
    {
        if (!items.Contains(thing)) { items.Add(thing); }
    }

    public override EaterCard GetItem(int index)
    {
        return items[index];
    }

    public override void Remove(EaterCard thing)
    {
        if (!items.Contains(thing)) { items.Remove(thing); }
    }

}
