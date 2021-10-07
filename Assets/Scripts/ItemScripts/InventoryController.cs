using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
    Controls interactions between a player and the inventory
*/
public class InventoryController
{
    public Player player;
    public PlayerInventory inventory;
    public UI_Inventory inventoryUI;
    public UI_Hotbar hotbarUI;

    public Item selectedItem;

    public void OnInventoryRightClick(Item clickedItem, int slotIndex) {
        if(clickedItem == selectedItem) {
            selectedItem.Equip(player, false); //Trigger unequip behaviour
            selectedItem = null;
        }

        Collectable spawnedItem = inventory.DropItem(player.transform.position, slotIndex);
        spawnedItem.ApplyRandomForce(11.0f);
    }


    public void OnHotbarLeftClick(Item clickedItem, int slotIndex) {
        //Handle equipped item logic
        Item previousItem = selectedItem;
        selectedItem = (clickedItem == previousItem) ? null : clickedItem; //Update selected item

        if(selectedItem != null) {
            selectedItem.Equip(player, true); //Trigger equip behaviour for the newly selected item
        }

        if(previousItem != null) {
            previousItem.Equip(player, false); //Trigger unequip behaviour for the previously selected item
        }

        player.UpdateItemAnimations();
    }

    public void OnInventoryLeftClick(Item clickedItem, int slotIndex) {
        if(inventory.IsHotbarFull()) //Do not move items to hotbar if it is already full
            return;

        OnHotbarLeftClick(clickedItem, slotIndex);
        inventory.TransferToHotbar(slotIndex); //Move selected item to hotbar
        hotbarUI.SetSelectedIndex(inventory.GetNumHotbarItems() - 1);

        player.UpdateItemAnimations();
    }

    public void OnHotbarRightClick(Item clickedItem, int slotIndex) {
        if(clickedItem == selectedItem) {
            selectedItem.Equip(player, false);
            selectedItem = null;
        }

        inventory.TransferFromHotbar(slotIndex);
        player.UpdateItemAnimations();
    }
}
