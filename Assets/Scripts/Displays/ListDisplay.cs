using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ListDisplay : MonoBehaviour
{
    private Scene currentScene;
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            for (int i =0; i < this.transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                Destroy(child);
            }
            SceneManager.UnloadSceneAsync("EatenlistDisplay");
            Destroy(this.gameObject);
        }
    }
}
