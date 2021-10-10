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

    private bool isVisible;
    void Awake()
    {
        this.popupCanvas = GetComponent<Canvas>();
        this.popupBackground = GameObject.Find("PopupBackground").GetComponent<Image>();
        this.popupContent = GameObject.Find("PopupContent").GetComponent<Image>();
        this.exitButton = GameObject.Find("ExitButton").GetComponent<Button>();

        SetVisible(true);
    }

    public void SetVisible(bool visible)
    {
        isVisible = visible;
        popupCanvas.enabled = isVisible;
    }

    public void toggleVisible()
    {
        SetVisible(!isVisible);
    }

    public void exitPopup()
    {
        toggleVisible();
    }
}
