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
    [SerializeField] private bool feeding;
    [SerializeField] private int eatersAlive;
    [SerializeField] private int eatersFull;

    [Header("Game Attributes that must be reset at the end of the game")]
    [SerializeReference] private int numOfInstantiatedEaters;

    // Public Variables //
    public int NumOfInstantiatedEaters { get { return numOfInstantiatedEaters; } private set { numOfInstantiatedEaters = value; } }
    public int EaterCount { get { return items.Count; } private set { } }

    /// <summary>
    /// Method to update the number of instantiatedEater and eatersAlive in eaterList; Also method to change turn 0 to turn 1 after all eater gets instantiated
    /// </summary>
    public void NotifyInstantiated() 
    {
        this.numOfInstantiatedEaters++;
        this.eatersAlive++;
        
        if (numOfInstantiatedEaters == gameVar.GetNUM_OF_EATERS())
        {
            gameVar.StartFirstTurn();
        }
    }

    // === Setters and Getters Methods === //

    /// <summary>
    /// Adds an eaterCard to the eaterList
    /// </summary>
    /// <param name="eaterCard"></param>
    /// <exception cref="System.Exception"></exception>
    public override void Add(EaterCard eaterCard)
    {
        if (!items.Contains(eaterCard) && items.Count < gameVar.GetNUM_OF_EATERS()) { items.Add(eaterCard); }
        else if (!items.Contains(eaterCard) && items.Count > gameVar.GetNUM_OF_EATERS()) { throw new System.Exception("Too many Eaters"); }
    }
    /// <summary>
    /// Returns an eater from eaterList
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public override EaterCard GetItem(int index) {return items[index];}
    /// <summary>
    /// Removes an eater from eaterList
    /// </summary>
    /// <param name="eaterCard"></param>
    public override void Remove(EaterCard eaterCard) {items.Remove(eaterCard);}
    /// <summary>
    /// Returns a bool that tells if the player is currently feeding any eater cards
    /// </summary>
    /// <returns></returns>
    public bool IsFeeding() { return feeding; }
    /// <summary>
    /// Returns the numbers of eaters alive
    /// </summary>
    /// <returns>int</returns>
    public int GetNumOfEatersAlive() { return eatersAlive; }
    /// <summary>
    /// Returns a list of all eaters that are alive
    /// </summary>
    /// <returns>List<EaterCard></EaterCard></returns>
    public List<EaterCard> GetAllAliveEater() 
    {
        List<EaterCard> ls = new List<EaterCard>();
        foreach (EaterCard eater in items) { if (eater.isActiveAndEnabled) { ls.Add(eater); } }
        return ls;
    }
    /// <summary>
    /// A setter that is used after a eater card is deemed dead to update the number of eaters alive field in eaterList
    /// </summary>
    public void SubtractNumOfEaterAlive() { eatersAlive--; }
    /// <summary>
    /// Get the total number of eaters that are full
    /// </summary>
    /// <returns>int</returns>
    public int GetNumOfEaterFull() { return eatersFull; }
    /// <summary>
    /// A setter that is used when an eater is fed to full to update the num of eater full field in eaterList
    /// </summary>
    public void IncrementEaterFull() { eatersFull++; }

    // === Data Reset between turns/rounds == //

    private void OnValidate() { ResetDataBetweenGames();}

    /// <summary>
    /// A setter that reset the eaterFull count which typically gets run after every turn to reset fullness of eaters
    /// </summary>
    public void ResetEaterFullCount() { eatersFull = 0; }

    /// <summary>
    /// Reset data between rounds
    /// </summary>
    public void ResetRoundStats()
    {
        this.numOfInstantiatedEaters = 0;
    }

    /// <summary>
    /// Method to reset essental data between games
    /// </summary>
    public void ResetDataBetweenGames()
    {
        items.Clear();
        numOfInstantiatedEaters = 0;
        eatersAlive = 0;
        eatersFull = 0;
        feeding = false;
    }

    // === Methods relates to feeding === //

    /// <summary>
    /// Updates the isFeeding state of the EaterList. Mainly used for stopping player from feeding halfway to clear their hand and then draw more cards
    /// </summary>
    public void FeedingUpdate()
    {
        foreach (EaterCard eater in items)
        {
            if (eater.GetIsFull() != true && (eater.GetHungerValue() != 0) && (eater.GetHungerValue() < eater.GetCardValue()))
            {
                feeding = true;
                return;
            }
        }
        feeding = false;

    }
}
