using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "ListDisplayGUISystem", menuName = "ScriptableObjects/ListDisplayGUISystem")]
public class ListDisplayGUISystem : ScriptableObject
{
    [SerializeField] private GameObject displayPrefab;
    [SerializeField] private CardsInUse cardsInUse;
    [SerializeField] private GameObject cardImageDisplay;
    private GameObject listDisplay;

    public void ShowList(List<string> ls)
    {
        SceneManager.LoadScene("EatenlistDisplay", LoadSceneMode.Additive);
        listDisplay = Instantiate(displayPrefab);
        listDisplay = listDisplay.transform.GetChild(1).gameObject;

        foreach (string cardName in ls)
        {
            Sprite cardSprite = cardsInUse.getCardSprite(cardName);
            GameObject cardImg = Instantiate(cardImageDisplay);
            cardImg.GetComponent<UnityEngine.UI.Image>().sprite = cardSprite;
            cardImg.transform.SetParent(listDisplay.transform);
        }
        listDisplay = listDisplay.transform.parent.gameObject;
        SceneManager.MoveGameObjectToScene(listDisplay, SceneManager.GetSceneByName("EatenlistDisplay"));
    }
}
