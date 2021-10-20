using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public Canvas popupCanvas;
    public Image popupBackground;
    public Image popupContent;
    public Button exitButton;
    void Awake()
    {
        PauseGame();
        this.popupCanvas = GetComponent<Canvas>();
        this.popupBackground = GameObject.Find("PopupBackground").GetComponent<Image>();
        this.popupContent = GameObject.Find("PopupContent").GetComponent<Image>();
        this.exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
    }

    public void ExitPopup()
    {
        ResumeGame();
        Destroy(this.gameObject);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
