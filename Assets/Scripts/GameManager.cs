using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    int gameInt;
    Inventory inventory;
    int restaurantRating;

    private GameManager() {
        this.inventory = new Inventory();
    }

    void Awake() {
        instance = new GameManager();
    }

    public Inventory GetInventory() {
        return instance.inventory;
    }
}
