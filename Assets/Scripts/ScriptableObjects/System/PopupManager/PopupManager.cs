using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "PopupManager", menuName = "ScriptableObjects/PopupManager")]
public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject DrawPhaseNotification;
    [SerializeField] private GameObject ActionPhaseNotification;

    public void test()
    {
        Debug.Log("Test");
        StartCoroutine(NotifyDrawPhase(2));
        //NotifyDrawPhase(2);
    }

    public IEnumerator NotifyDrawPhase(int waitTime)
    {
        Debug.Log("NotifyDrawPhase was called");
        DrawPhaseNotification.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        DrawPhaseNotification.SetActive(false);
    }

    public IEnumerator NotifyActionPhase(int waitTime)
    {
        ActionPhaseNotification.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        ActionPhaseNotification.SetActive(false);
    }
}
