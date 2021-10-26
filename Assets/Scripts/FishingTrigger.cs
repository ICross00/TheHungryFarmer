using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingTrigger : PopupTrigger
{
    string playerItemHeld;
    Player player;

    protected override void OnTriggerEnter2D(Collider2D collide)
    {
        if (collide.name == "Player")
        {
            player = collide.GetComponent<Player>();
            if (player.GetSelectedItem() != null)
                playerItemHeld = player.GetSelectedItem().GetItemType().ToString();
            else
                playerItemHeld = null;

            if (playerItemHeld == "FishingRod")
            {
                GameObject tempObj = Resources.Load<GameObject>("Prefabs/Popups/FishingCanvas Variant");
                popupWindow = GameObject.Instantiate(tempObj, Vector3.zero, Quaternion.identity);
                popup = popupWindow.GetComponent<Popup>();
            }
        }
    }
}
