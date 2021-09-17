using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interactable : MonoBehaviour
{
    public float interactionRadius = 1.0f;

    /**
        Finds all of the Interactable objects whose collider components overlap the provided position,
        and returns them all as a List<Interactable>

        @param position The position to check for Interactable objects at
        @return A list containing all of the found Interactable objects
    */
    public static List<Interactable> GetInteractablesAtPoint(Vector2 position) {
        List<Interactable> results = new List<Interactable>();

        //Get all of the colliders at this point
        Collider2D[] collidersAtPoint = Physics2D.OverlapCircleAll(position, .05f);
        if(collidersAtPoint.Length > 0) {
            foreach(Collider2D collider in collidersAtPoint) { //Iterate over each collider and find those that are Interactables
                Interactable interactable = collider.GetComponent<Collider2D>().GetComponent<Interactable>();
                if(interactable != null) {
                    results.Add(interactable); //Add the interactable to the results list
                }
            }
        }

        return results;
    }

    /**
    This function should be called on an Interactable object whenever the programmer
    wants to trigger its interactable behaviour.

    @param triggerPlayer The Player that triggered this interaction. 
    */
    public virtual void Interact(Player triggerPlayer) { }

    /**
    This function should be called on an Interactable object whenever the programmer
    wants to trigger its interactable behaviour.

    @param triggerPlayer The Player that triggered this interaction. 
    */
    public virtual void DisableInteract(Player triggerPlayer) { }
}