using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardScriptableObject", menuName = "ScriptableObjects/Card")]
public class CardScriptableObject : ScriptableObject
{
    public Sprite CardSprite { get; private set; }
    public Sprite CardBackSprite { get; private set; }
    public CardTypeEnumScriptableObject CardType { get; private set; }
    public int CardValue { get; private set; }    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
