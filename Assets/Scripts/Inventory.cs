using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event EventHandler OnItemListChanged;
    public List<Item> items;

    /*
        Returns the inventory as a list of items
    */
    public List<Item> GetItemList() {
        return items;
    }

    /*
    Adds an item to the inventory
    Todo: Decrement the stack of the world item being picked up, this is important if the inventory is near
    full and only part of the stack can be picked up
    */
    public void AddItem(Item newItem) {
        //Attempt to insert each item in the stack
        for(int stack = 0; stack < newItem.amount; stack++) {
            bool canStack = false;
            //Check if the item can be stacked first
            foreach(Item storeditem in items) {
                //If the items are of the same type and if the stored item is below the max stack size, then the item can be stacked
                if(storeditem.GetItemType() == newItem.GetItemType() && storeditem.amount < storeditem.item.maxStack) {
                    storeditem.amount += 1; //Increment the stack
                    canStack = true;
                    break;
                }
            }

            if(!canStack) { //If the item cannot be stacked, add it to the end of the inventory
                items.Add(new Item { item = newItem.item, amount = 1 } );
            }
        }


        //Notify any listeners that the inventory changed
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    /*
    Remove an item from an inventory slot
    @param slotIndex The index of the item slot to remove an item from
    @return The item that was removed
    */
    public Item RemoveItem(int slotIndex) {
        try
        {
            Item droppedItem = items[slotIndex];
            items.RemoveAt(slotIndex);

            return droppedItem;
        }
        catch (IndexOutOfRangeException e) 
        {
            Debug.Log(e.Message);
            throw new ArgumentOutOfRangeException("Tried to remove from an out of range inventory slot: " + slotIndex.ToString(), e);
        }
    }

    /**
    Drops an item from an inventory slot, spawning a collectable in the world
    @param position The position in the world to spawn the collectable
    @param slotIndex The index of the item slot to remove an item from
    @return The collectable that was spawned
    */
    public Collectable DropItem(Vector3 position, int slotIndex) {
        Item droppedItem = RemoveItem(slotIndex);
        Collectable spawnedItem = Collectable.Spawn(position, droppedItem, 1.5f);
        return spawnedItem;
    }

}
