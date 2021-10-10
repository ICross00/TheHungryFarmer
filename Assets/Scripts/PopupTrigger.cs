using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    private GameObject popupWindow;
    private Popup popup;
    public string popupName;

    public void OnTriggerEnter2D(Collider2D collide)
    {
        Debug.Log("PopupTrigger Entered");
        if (collide.name == "Player")
        {
            popupWindow = Resources.Load<GameObject>("Prefabs/Popups/PopupCanvas");
            popup = popupWindow.GetComponent<Popup>();
        }
    }
}
