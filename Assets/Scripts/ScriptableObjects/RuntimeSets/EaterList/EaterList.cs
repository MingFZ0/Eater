using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* One limitation of this technique is that if you inspect the ScriptableObject at runtime, you won’t be able to see the 
 * contents of the Items list in the Inspector by default. Instead, a “Type mismatch” appears in each element field. 
 * By design, a ScriptableObject can’t serialize a scene object. The list is actually working, but the data does not display correctly.
 */

[CreateAssetMenu(fileName = "EaterList", menuName = "ScriptableObjects/RuntimeSet/EaterList")]

/*** <summary> This is a class meant to represent a list of all eaters that are still ACTIVE and are ALIVE in the scene. ***/
public class EaterList : RuntimeSetSO<EaterCard>
{
    [SerializeField] private GameVariables gameVar;
    [SerializeField] private bool isFeeding;
    [SerializeField] private int eatersAlive;
    [SerializeField] private int eatersFull;


    [Header("Game Attributes that must be reset at the end of the game")]
    [SerializeReference] private int numOfInstantiatedEaters;
    public int NumOfInstantiatedEaters { get { return numOfInstantiatedEaters; } private set { numOfInstantiatedEaters = value; } }


    public int EaterCount
    { get { return items.Count; } private set { } }

    public void NotifyInstantiated() 
    {
        this.numOfInstantiatedEaters++;
        this.eatersAlive++;
        
        if (numOfInstantiatedEaters == gameVar.GetNUM_OF_EATERS())
        {
            gameVar.StartFirstTurn();
        }
    }


    public override void Add(EaterCard eaterCard)
    {
        if (!items.Contains(eaterCard) && items.Count < gameVar.GetNUM_OF_EATERS()) { items.Add(eaterCard); }
        else if (!items.Contains(eaterCard) && items.Count > gameVar.GetNUM_OF_EATERS()) { throw new System.Exception("Too many Eaters"); }
    }
    public override EaterCard GetItem(int index) {return items[index];}
    public override void Remove(EaterCard eaterCard) {items.Remove(eaterCard);}
    public bool GetIsFeeding() { return isFeeding; }
    public void Clear() { items.Clear(); }
    public int GetNumOfEatersAlive() { return eatersAlive; }
    public List<EaterCard> GetAllAliveEater() 
    {
        List<EaterCard> ls = new List<EaterCard>();
        foreach (EaterCard eater in items) { if (eater.isActiveAndEnabled) { ls.Add(eater); } }
        return ls;
    }
    public void SubtractNumOfEaterAlive() { eatersAlive--; }
    public int GetNumOfEaterFull() { return eatersFull; }
    public void IncrementEaterFull() { eatersFull++; }
    public void ResetEaterFullCount() { eatersFull = 0; }

    public void ResetRoundStats()
    {
        this.numOfInstantiatedEaters = 0;
    }


    public void FeedingUpdate()
    {
        foreach (EaterCard eater in items)
        {
            if (eater.GetIsFull() != true && (eater.GetHungerValue() != 0) && (eater.GetHungerValue() < eater.GetCardValue()))
            {
                isFeeding = true;
                return;
            }
        }
        isFeeding = false;

    }

    public void ResetData()
    {
        items.Clear();
        numOfInstantiatedEaters = 0;
        eatersAlive = 0;
        eatersFull = 0;
        isFeeding = false;
    }

    private void OnValidate()
    {
        ResetData();
    }
}
