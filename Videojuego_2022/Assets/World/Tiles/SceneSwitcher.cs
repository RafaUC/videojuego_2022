using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void OpenScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() //Por ahora no hace nada, pero cuando importemos el videojuego si va a cerrar el programa
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
