using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSceen : MonoBehaviour
{
    [SerializeReference] private string pauseMenu = "Pause";
    public bool paused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                SceneManager.LoadScene(pauseMenu, LoadSceneMode.Additive);
                paused = true;
            } else
            {
                SceneManager.UnloadSceneAsync(pauseMenu);
                paused = false;

            }
        }
    }
}
