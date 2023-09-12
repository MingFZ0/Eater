using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update

    Sprite cardSprite;

    public int CardValue;
    public CardProperties.cardTypes CardType;

    public Color FaceUpColor;
    public Color FaceDownColor;

    public Vector2 prevPos;
    public bool Selected;
    public bool Eater = false;
    public bool EaterKilled = false;
    public bool stored = false;

    System.Random randomNumGen = new System.Random();

    [SerializeField] TextMesh displayText;

    private void Awake()
    {
        if (this.name == "Card" || this.name == "Card(Clone)")
        {
            int cardTypeID = randomNumGen.Next(0, 4);
            CardType = (CardProperties.cardTypes)cardTypeID;
            CardValue = randomNumGen.Next(1, 14);
            this.name = $"{CardValue.ToString()}_{CardType}";
        }
        
    }

    private void OnDestroy()
    {
        HandManager.CardsInHand.Remove(this);
    }
    //void OnEnable() => HandManager.CardsInHand.Add(this);
    //void OnDisable() => HandManager.CardsInHand.Remove(this);

    void Start()
    {
        displayText.text = CardValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        //Destroy(this);
        
    }
}
