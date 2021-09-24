using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game has closed");
    }
}
