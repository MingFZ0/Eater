using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrizeCardList", menuName = "ScriptableObjects/RuntimeSet/PrizeCardList")]
public class PrizeCardList : RuntimeSetSO<PrizeCard>
{
    public PrizeCard revealed;

    public int PrizeCardsCount
    { get { return items.Count; } private set { } }

    private void OnValidate()
    {
        revealed = null;
        items.Clear();
    }

    public override void Add(PrizeCard thing)
    {
        if (!items.Contains(thing)) { items.Add(thing); }
    }

    public override PrizeCard GetItem(int index)
    {
        return items[index];
    }

    public override void Remove(PrizeCard thing)
    {
        if (items.Contains(thing)) { items.Remove(thing); }
    }

}
