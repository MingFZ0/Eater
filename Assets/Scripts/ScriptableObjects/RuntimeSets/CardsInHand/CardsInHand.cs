using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/* One limitation of this technique is that if you inspect the ScriptableObject at runtime, you won’t be able to see the 
 * contents of the Items list in the Inspector by default. Instead, a “Type mismatch” appears in each element field. 
 * By design, a ScriptableObject can’t serialize a scene object. The list is actually working, but the data does not display correctly.
 */

[CreateAssetMenu(fileName = "CardsInHand", menuName = "ScriptableObjects/RuntimeSet/CardsInHand")]

public class CardsInHand : RuntimeSetSO<Card>
{
    [SerializeField] public int handLength { get { return items.Count; } private set { } }
    [SerializeReference] public Card selectedCard;
    [SerializeField] private float displayBoxWidth;
    [SerializeField] private float displayBoxYCoord;

    [SerializeField] private bool displayBoxOutline;


    public override void Add(Card card)
    {
        if (!items.Contains(card) && !ContainSameValueInList(card))
        {
            items.Add(card);
            //Debug.Log("card added to hand");
        } else { throw new System.Exception(card + " is already found in list. Unable to add it"); }
    }
    public override void Remove(Card card)
    {
        if (items.Contains(card)) { 
            items.Remove(card);
        }
        else { throw new System.Exception("Unable to find " + card + " in list"); }
    }
    public void Destory(Card card)
    {
        if (items.Contains(card))
        {
            Destroy(card);
        }
        
    }


    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneUpdate;
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneUpdate;
    }
    
    private void OnSceneUpdate(SceneView sceneView)
    {
        if (displayBoxOutline == false) { return; }
        Handles.color = Color.green;
        Handles.DrawSolidRectangleWithOutline(new Rect(0 - (displayBoxWidth / 2), displayBoxYCoord + (displayBoxYCoord * 0.55f), displayBoxWidth, 3), Color.clear, Color.green);
    }

    public override Card GetItem(int index) {
        Debug.Log("DEBUG -GetItem--> Index is " + index);
        Debug.Log("DEBUG -GetItem--> Size of items is " + items.Count);
        Debug.Log("DEBUG -GetItem--> Items is " + items[index]);
        return items[index];
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


    public void UpdateHandDisplay()
    {
        float currentZCord = 0 + items.Count;

        float distanceBetweenEachCard = displayBoxWidth / (items.Count);
        float currentIteratingPos = distanceBetweenEachCard - (displayBoxWidth / 2);
        currentIteratingPos -= (distanceBetweenEachCard / 2);

        foreach (Card card in items)
        {
            card.transform.position = new Vector3(currentIteratingPos, displayBoxYCoord, currentZCord);
            card.previousPos = new Vector3(currentIteratingPos, displayBoxYCoord, currentZCord);
            currentIteratingPos += distanceBetweenEachCard;
            currentZCord--;
        }
    }

    public void Clear() 
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            Card card = items[i];
            Destroy(card.gameObject);
        }
        items.Clear(); 

    }

    private void OnValidate()
    {
        selectedCard = null;
    }
}
