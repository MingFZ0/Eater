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
    [SerializeField] private GameVariables gameVar;
    public int NumOfInstantiatedEaters { get { return numOfInstantiatedEaters; } private set { numOfInstantiatedEaters = value; } }
    [SerializeReference] private int numOfInstantiatedEaters;

    public int EaterCount
    { get { return items.Count; } private set { } }

    private void OnValidate()
    {
        Debug.Log("Reset");
        numOfInstantiatedEaters = 0;
    }

    public void NotifyInstantiated() 
    {
        Debug.Log("An Eater has been instantiated");
        this.numOfInstantiatedEaters++;
        
        if (numOfInstantiatedEaters == gameVar.GetNUM_OF_EATERS())
        {
            Debug.Log("Start first turn");
            gameVar.StartFirstTurn();
        }
    }

    public override void Add(EaterCard eaterCard)
    {
        if (!items.Contains(eaterCard) && items.Count < gameVar.GetNUM_OF_EATERS()) { items.Add(eaterCard); }
        else if (!items.Contains(eaterCard) && items.Count > gameVar.GetNUM_OF_EATERS()) { throw new System.Exception("Too many Eaters"); }
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
