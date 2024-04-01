using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TooltipSystem", menuName = "ScriptableObjects/TooltipSystem")]
public class TooltipSystem : ScriptableObject
{
    public Tooltip tooltip;

    public void Show() {tooltip.gameObject.SetActive(true);}

    public void Hide() {tooltip.gameObject.SetActive(false);}

}
