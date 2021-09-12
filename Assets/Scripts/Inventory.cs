using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public event EventHandler OnItemListChanged;
    private List<Item> items;

    void Awake() {
        items = new List<Item>();
        Debug.Log("Inventory initialized");
    }

    public List<Item> GetItemList() {
        return items;
    }

    public void AddItem(Item item) {
        bool canStack = false;
        //Check if the item can be stacked first
        foreach(Item storeditem in items) {
            if(storeditem.type == item.type) {
                storeditem.amount += 1; //Increment the amount
                canStack = true; 
                break;
            }
        }

        if(!canStack) { //If the item cannot be stacked, add it to the end of the inventory
            items.Add(item);
        }

        //Notify any listeners that the inventory changed
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
}
