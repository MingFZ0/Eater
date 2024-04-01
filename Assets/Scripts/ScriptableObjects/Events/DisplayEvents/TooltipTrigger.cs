using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TooltipSystem tooltipSys;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipSys.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipSys.Hide();
    }
}
