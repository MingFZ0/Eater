using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameVariables gameVar;
     public void loadScene()
    {
        if (SceneManager.GetActiveScene().name == "Pause") { SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()); }
        if (sceneName == "Classic") {gameVar.resetData();}
        SceneManager.LoadScene(sceneName);
    }
}
