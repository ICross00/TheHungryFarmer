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
            if (true)
            {
                GameObject tempObj = Resources.Load<GameObject>("Prefabs/Popups/" + popupName);
                popupWindow = GameObject.Instantiate(tempObj, Vector3.zero, Quaternion.identity);
                popup = popupWindow.GetComponent<Popup>();
            }
        }
    }
}
