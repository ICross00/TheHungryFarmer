using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Interactable : MonoBehaviour
{
    //Number of seconds between player collision checks
    private static float COLLISION_CHECK_INTERVAL = 0.1f;
    private static float cCheckTime;

    protected Player interactingPlayer; //The player that this interactable is currently in interaction with

    void OnEnable() {
        cCheckTime = Time.time + COLLISION_CHECK_INTERVAL;
        StartCoroutine(CheckInCollision());
    }

    /**
        Finds all of the Interactable objects whose collider components overlap the provided circle,
        defined by the position and radius, and returns them all as a List<Interactable>

        @param position The position to check for Interactable objects at
        @param searchRadius The radius around the position to search for Interactable objects in
        @return A list containing all of the found Interactable objects
    */
    public static List<Interactable> GetInteractablesInRadius(Vector2 position, float searchRadius) {
        List<Interactable> results = new List<Interactable>();

        //Get all of the colliders at this point
        Collider2D[] collidersAtPoint = Physics2D.OverlapCircleAll(position, searchRadius);
        if(collidersAtPoint.Length > 0) {
            foreach(Collider2D collider in collidersAtPoint) { //Iterate over each collider and find those that are Interactables
                Interactable interactable = collider.GetComponent<Interactable>();
                if(interactable != null) {
                    results.Add(interactable); //Add the interactable to the results list
                }
            }
        }
        //results.Sort();
        return results;
    }

    /**
    This function should be called on an Interactable object whenever the programmer
    wants to trigger its interactable behaviour.

    @param triggerPlayer The Player that triggered this interaction. 
    */
    public void Interact(Player triggerPlayer) {
        //Call child functionality
        OnInteract(triggerPlayer);
        interactingPlayer = triggerPlayer;
    }

    /**
    This function should be called on an Interactable object whenever the programmer
    wants to disable its interactable behaviour. This is useful for closing interactable
    dialogs like inventory screens or popups when the player walks too far away.

    @param triggerPlayer The Player that triggered this interaction. 
    */
    public void Close(Player triggerPlayer) {
        //Call child functionality
        OnClose(triggerPlayer);
        interactingPlayer = null;
    }

    protected abstract void OnInteract(Player triggerPlayer);

    protected virtual void OnClose(Player triggerPlayer) {}

    IEnumerator CheckInCollision() {
        while(true) {
            yield return new WaitForSeconds(COLLISION_CHECK_INTERVAL); //Only check every 0.1s to save resources

            if(this.interactingPlayer != null) {
                //Check if the player is still in collision with the interactable
                Collider2D playerCollider = interactingPlayer.GetComponent<Collider2D>();
                Collider2D interactableCollider = GetComponent<Collider2D>();

                float distance = interactableCollider.Distance(playerCollider).distance;
                //If they are not, trigger the onclose behaviour
                if(distance > 1.0f) {
                    Close(interactingPlayer);
                }
            }
        }
    }
}