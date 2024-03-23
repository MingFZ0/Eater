using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelect : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject source;

    public void revealTarget()
    {
        if (target.activeInHierarchy == false) { target.SetActive(true); }
        else { target.SetActive(false); }
        source.SetActive(false);
    }
}
