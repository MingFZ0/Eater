using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* One limitation of this technique is that if you inspect the ScriptableObject at runtime, you won’t be able to see the 
 * contents of the Items list in the Inspector by default. Instead, a “Type mismatch” appears in each element field. 
 * By design, a ScriptableObject can’t serialize a scene object. The list is actually working, but the data does not display correctly.
 */

[CreateAssetMenu(fileName = "Card RuntimeSet", menuName = "ScriptableObjects/RuntimeSet/CardRuntimeSet")]

public class CardRuntimeSetSO : RuntimeSetSO<Card>
{

}
