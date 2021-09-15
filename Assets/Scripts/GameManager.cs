using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Inventory playerInventory;

    int gameInt;
    int restaurantRating;

    //Decalring floating text for later use
    public FloatingTextManager floatingTextManager;

    //Ability to display floating text after being called with parameters.
    public void ShpwText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.show(msg, fontSize, color, position, motion, duration);
    }
}
