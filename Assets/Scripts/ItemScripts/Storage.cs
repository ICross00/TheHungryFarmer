using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Class representing storage such as a chest. This must be given a UI_Inventory object on which it will display the storage inventory.
    When a player interacts with something that causes the storage to open, the player object should be received using the
    OpenStorage function.
*/
public class Storage : Interactable
{
    //The items that this storage will sell
    private Inventory storageInventory;
    public UI_Inventory storageUI;

    private Player player;
    private Inventory playerInventory;
    private bool isStorageOpen = false;

    void Awake() {
        storageInventory = GetComponent<Inventory>();
        storageUI.SetInventory(storageInventory);

        //Set up callback functions for when an item is retrieved

        storageUI.onButtonRightClicked.AddListener((Item item, int slotIndex) => {
            if(player != null) {
                RetrieveItem(item);
            }
        });
    }

    /**
    Displays a storage screen to a player, who is treated as the storage's user until the storage is closed
    @param p The player to display the storage to
    */
    public void OpenStorage(Player p) {
        player = p;
        playerInventory = p.GetInventory();

        storageUI.SetInventory(storageInventory);
        storageUI.SetVisible(true);

        //We need to temporarily override the player's UI inventory right click function to make it deposit items in the inventory rather than dropping them
        UI_Inventory playerInvUI = player.inventoryUI;
        playerInvUI.onButtonRightClicked.RemoveAllListeners();
        playerInvUI.onButtonRightClicked.AddListener((Item item, int slotIndex) => {
            //Override the right click event to store the item in the storage
            StoreItem(item);
        });

        isStorageOpen = true;
    }

    /**
    Closes the storage and removes the player
    Resets the UI inventory callback functions for the player
    */
    protected override void OnClose(Player triggerPlayer) {
        triggerPlayer.SetDefaultInventoryListeners();

        player = null;
        playerInventory = null;

        storageUI.SetVisible(false);
        isStorageOpen = false;
    }

    protected override void OnInteract(Player triggerPlayer) {
        if(isStorageOpen) {
            Close(triggerPlayer);
        } else {
            OpenStorage(triggerPlayer);
        }
    }

    /**
    Retrieves an item by removing the provided item from the storage and adding it to the player's inventory
    @param retrievedItem The item to retrieve
    @return True if the player could sell the selected item, false otherwise
    */
    public bool RetrieveItem(Item retrievedItem) {
        Item itemCopy = Item.CopyItem(retrievedItem);
        if(storageInventory.RemoveItem(retrievedItem)) {
            playerInventory.AddItem(itemCopy);
            return true;
        }

        return false;
    }

    /**
    Stores an item by removing the provided item from the player's inventory and adding it to the storage
    @param storedItem the item to store
    @return True if the item could be stored, false otherwise
    */
    public bool StoreItem(Item storedItem) {
        Item itemCopy = Item.CopyItem(storedItem);
        if(playerInventory.RemoveItem(storedItem)) {
            storageInventory.AddItem(itemCopy);
            return true;
        }

        return false;
    }

}
