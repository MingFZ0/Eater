using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* One limitation of this technique is that if you inspect the ScriptableObject at runtime, you won’t be able to see the 
 * contents of the Items list in the Inspector by default. Instead, a “Type mismatch” appears in each element field. 
 * By design, a ScriptableObject can’t serialize a scene object. The list is actually working, but the data does not display correctly.
 */

[CreateAssetMenu(fileName = "CardScriptableObject", menuName = "ScriptableObjects/Card")]
public class CardScriptableObject : ScriptableObject
{
    public Sprite CardSprite { get; private set; }
    public Sprite CardBackSprite { get; private set; }
    public CardTypeEnumScriptableObject CardType { get; private set; }
    public int CardValue { get; private set; }    

}
