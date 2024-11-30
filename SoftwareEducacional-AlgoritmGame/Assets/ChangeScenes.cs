using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        // Verifica se a cena existe e então a carrega
        if (SceneManager.GetSceneByName(sceneName) != null)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("A cena " + sceneName + " não foi encontrada!");
        }
    }
}


