using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public Sprite openSprite;
    public Sprite closedSprite;
    public bool isOpen = false;

    void Start() {
        SetDoorOpen(isOpen);
    }

    /*
    Sets whether or not the door is open. This will also update the door's sprite
    and collision to match the new open/closed state
    @param open True to open the door, false to close the door
    */
    void SetDoorOpen(bool open) {
        isOpen = open;

        //Update sprite
        SpriteRenderer doorRenderer = GetComponent<SpriteRenderer>();
        doorRenderer.sprite = isOpen ? openSprite : closedSprite;

        //Update door collisionss
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = isOpen;
    }

    //Interact function - open/close the door by toggling
    protected override void OnInteract(Player triggerPlayer) {
        SetDoorOpen(!isOpen);
    }
}
