using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
    Controls interactions between a player and the inventory
*/
public class InventoryUIController
{
    public Player player;
    public PlayerInventory inventory;
    public UI_Inventory inventoryUI;
    public UI_Hotbar hotbarUI;

    public Item selectedItem;

    public void ShowHideInventory() {
        if(inventoryUI) {
            player.isInvOpen = !player.isInvOpen;
            inventoryUI.ToggleVisible();
        }
    }

    public void UseSelectedItem() {
        if(selectedItem != null) {
            if(selectedItem.amount > 0)
                selectedItem.Use(player);

            if(selectedItem.amount <= 0) { //Deselect the item
                selectedItem = null;
                hotbarUI.SetSelectedIndex(-1);
                player.UpdateItemAnimations();
            }
        }
    }

    /*
    Selects an item from the hotbar
    @param hotbarIndex The index in the hotbar to select
    */
    public void SelectHotbarItem(int hotbarIndex) {
        Item item = inventory.GetItemFromHotbar(hotbarIndex);
        if (item == null) //Do nothing if an out of range item was selected
            return;

        SelectItem(item);
        hotbarUI.SetSelectedIndex(selectedItem == null ? -1 : hotbarIndex); //If the item was deselected, also deselect the hotbar item
    }

    /*
    Forcibly selects an item, overriding whatever was previously selected
    If the selected item was already selected, then it will be deselected

    @param item The item to select
    */
    private void SelectItem(Item item) {
        Item previousItem = selectedItem;
        selectedItem = (item == previousItem) ? null : item; //Update selected item

        if(selectedItem != null)
            selectedItem.Equip(player, true); //Trigger equip behaviour for the previously selected item

        if(previousItem != null)
            previousItem.Equip(player, false); //Trigger unequip behaviour for the previously selected item

        player.UpdateItemAnimations();
    }

    /*
    The below functions should be passed in as parameters to the SetClickListeners function of a UI_Inventory class.
    They should not be called directly, as SetClickListeners will dynamically provide the parameters based on what the player
    clicked on in an inventory/hotbar
    */
    public void OnInventoryRightClick(Item clickedItem, int slotIndex) {
        if(clickedItem == selectedItem) {
            selectedItem.Equip(player, false); //Trigger unequip behaviour
            selectedItem = null;
        }

        Collectable spawnedItem = inventory.DropItem(player.transform.position, slotIndex);
        spawnedItem.ApplyRandomForce(10.0f);
    }


    public void OnHotbarLeftClick(Item clickedItem, int slotIndex) {
        SelectItem(clickedItem);
        player.UpdateItemAnimations();
    }

    public void OnInventoryLeftClick(Item clickedItem, int slotIndex) {
        if(inventory.IsHotbarFull()) //Do not move items to hotbar if it is already full
            return;

        SelectItem(clickedItem);
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
