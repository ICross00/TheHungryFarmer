using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event EventHandler OnItemListChanged;
    [SerializeField]
    private List<Item> items;

    void Start() {
         OnItemListChanged += Inventory_OnItemListChanged;
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e) {
        items.RemoveAll(storedItem => storedItem.amount == 0);
    }


    /*
    Returns the inventory as a list of items test
    */
    public List<Item> GetItemList() {
        return items;
    }

    /*
    Gets a specific item from the item list
    @param index The index of the item to retrieve
    @return The item at that index, or null if there was no item
    */
    public Item GetItem(int index) {
        return items[index];
    }

    /*
    Overrides the inventory with a list of items
    @param newItems The new items to override the inventory with
    */
    public void SetItemList(List<Item> newItems) {
        items = newItems;

        //Notify any listeners that the inventory changed
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
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
            for (int i = items.Count-1; i >= 0; i--) {
                Item storedItem = items[i];
                //If the items are of the same type and if the stored item is below the max stack size, then the item can be stacked
                if(storedItem.GetItemType() == newItem.GetItemType() && storedItem.amount < storedItem.itemTemplate.maxStack) {
                    storedItem.amount += 1; //Increment the stack
                    canStack = true;
                    break;
                }
            }

            if(!canStack) { //If the item cannot be stacked, add it to the end of the inventory
                Item clone = Item.CopyItem(newItem);
                clone.amount = 1;
                items.Add(clone);
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

            //Notify any listeners that the inventory changed
            OnItemListChanged?.Invoke(this, EventArgs.Empty);

            return droppedItem;
        }
        catch (IndexOutOfRangeException e) 
        {
            Debug.Log(e.Message);
            throw new ArgumentOutOfRangeException("Tried to remove from an out of range inventory slot: " + slotIndex.ToString(), e);
        }
    }

    /**
    Remove the first instance of an item from the inventory
    @param removedItem The item to remove from the inventory
    @return True if the item was successfully removed, false if the item was not in the inventory or if there was not enough of the item to remove the provided amount
    */
    public bool RemoveItem(ItemTemplate removedItem, int amount) {
        int index = 0;
        bool success = false;

        for (int i = items.Count-1; i >= 0; i--) {
            Item storedItem = items[i];
            //If the items are of the same type and if the amount of stored item is above the amount of the provided item, then the item can be removed
            if(storedItem.GetItemType() == removedItem.type && storedItem.amount >= amount) {
                storedItem.amount -= amount;
                if(storedItem.amount == 0) {
                    items.RemoveAt(i);
                }
                //Indicate success
                success = true;
                break;
            }

            index++;
        }

        //Notify any listeners that the inventory changed
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        return success;
    }

    /**
    Overload of RemoveItem(ItemTemplate, int)
    */
    public bool RemoveItem(Item removedItem) {
        return RemoveItem(removedItem.itemTemplate, removedItem.amount);
    }

    /**
    Removes a single instance of an item from the inventory
    @param removedItem The item to remove from the inventory
    @return True if the item was successfully removed, false if the item was not in the inventory
    */
    public bool RemoveItemSingle(Item removedItem) {
        Item singleItem = Item.CopyItem(removedItem);
        singleItem.amount = 1;
        return RemoveItem(singleItem);
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

    /*
        Converts the inventory into a string
        This can be deserialized into an List<Item> object using Item.FromString(string)
    */
    public override string ToString() {
        string result = "";

        foreach(Item storedItem in items) {
            result += ":" + storedItem.GetInternalName() + "," + storedItem.amount;
        }

        //Remove the first | before returning
        return result.Substring(1);
    }

    /*
    Checks if the inventory contains an item with a minimum amount
    @param item The item to check for in the inventory
    @param amount The amount of the item to check for
    @return True if the item existed in the inventory with the same amount or more, false if not
    */
    public bool ContainsItem(ItemTemplate item, int amount) {
        foreach(Item storedItem in items)
            if(storedItem.GetItemType() == item.type & storedItem.amount >= amount)
                return true;

        return false;
    }

    /*
    Parses a list of items from a serialized string. 
    This must be a string created using the ToString() method of the Inventory class

    If you want to update the inventory from the returned list, see the SetItemList method

    @param invAsString A string generated by the Inventory class's ToString method, representing a list of Item objects
    @return A list of Item objects corresponding to the provided string
    */
    public static List<Item> FromString(string invAsString) {
        List<Item> result = new List<Item>();

        string[] itemsAsStr = invAsString.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
        foreach(string item in itemsAsStr) {
            string[] itemInfo = item.Split(',');
            string _type = itemInfo[0];
            int _amount = int.Parse(itemInfo[1]);

            Item recreatedItem = Item.CreateItem(_type, _amount);
            result.Add(recreatedItem);
        }

        return result;
    }

}
