using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    Inventory playerInventory;

    //For now these values are zero or uninitialized. They will need to be loaded from a save state in the future
    int gameInt;
    int restaurantRating;
    int playerGold = 0;

    private void Awake()
    {
        instance = this;
        playerInventory = GetComponent<Inventory>();
    }

    public void ChangeGold(int amount) {
        playerGold += amount;
    }

    //Referencing floating text for later use
    public FloatingTextManager floatingTextManager;

    //Ability to display floating text after being called with parameters.
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }
}
