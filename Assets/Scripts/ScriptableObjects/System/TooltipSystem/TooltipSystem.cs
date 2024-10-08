using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TooltipSystem", menuName = "ScriptableObjects/TooltipSystem")]
public class TooltipSystem : ScriptableObject
{
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private GameObject cardImageDisplay;
    [SerializeField] private CardsInUse cardsInUse;

    private List<Card> feedingList;
    private GameObject displayTooltip = null;
    private bool display;

    public void Show(List<Card> newFeeding, Vector2 mousePos)
    {
        if (displayTooltip == null) { TooltipUpdate(newFeeding); }
        else if (equalsList(this.feedingList, newFeeding) == false) {Debug.Log("Teh two ls arent equal"); }

        if (display == false)
        {
            //displayTooltip.gameObject.SetActive(true);
            display = true;
        }
        displayTooltip.transform.position = Input.mousePosition;
    }

    public void Hide(List<Card> feedingList)
    {
        if (this.feedingList == null || this.displayTooltip == null) { return; }
        displayTooltip = displayTooltip.transform.parent.gameObject;
        Destroy(displayTooltip);
        displayTooltip = null;
        //displayTooltip.gameObject.SetActive(false);
        display = false;
    }

    private bool equalsList(List<Card> ls1, List<Card> ls2)
    {
        if (ls1.Count != ls2.Count) { return false; }
        for (int i = 0; i < ls1.Count; i++)
        {
            if (ls1[i].Equals(ls2[i]) == false) { return false; }
        }
        return true;
    }

    private void TooltipUpdate(List<Card> newFeeding)
    {
        displayTooltip = Instantiate(tooltipPrefab).transform.GetChild(0).gameObject;
        this.feedingList = newFeeding;

        foreach (Card card in feedingList)
        {
            Sprite cardSprite = cardsInUse.getCardSprite(card.name);
            GameObject cardImg = Instantiate(cardImageDisplay);
            cardImg.GetComponent<UnityEngine.UI.Image>().sprite = cardSprite;
            cardImg.transform.SetParent(displayTooltip.transform);
        }
        
    }

    private void OnValidate()
    {

        this.feedingList = null;
        this.display = false;
        this.displayTooltip = null;

    }

}
