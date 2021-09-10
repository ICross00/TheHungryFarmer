using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Inventory playerInventory;

    int gameInt;
    int restaurantRating;

    public static GameManager Instance {get; private set; }

    void Awake() {
        Instance = this;
        this.playerInventory = new Inventory(); //Later this will need to load the player's inventory from a saved state
    }

    public Inventory GetInventory() {
        return this.playerInventory;
    }
}
