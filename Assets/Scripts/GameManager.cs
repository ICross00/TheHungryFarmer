using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    Inventory playerInventory;

    int gameInt;
    int restaurantRating;

    //Referencing floating text for later use
    public FloatingTextManager floatingTextManager;

    //Ability to display floating text after being called with parameters.
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }
}
