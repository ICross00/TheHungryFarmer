using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : Interactable
{   
    private bool isOpen = false;
    public UI_CraftingStation craftingUI;
    public CraftingRecipe[] craftableItems; //An array of all recipes that can be crafted at this station
    private Inventory craftingInventory; //The inventory of the player currently interacting with the crafting station

    protected override void OnInteract(Player triggerPlayer) {
        if(isOpen) {
            OnClose(triggerPlayer);
        } else {
            craftingInventory = triggerPlayer.GetInventory();
            craftingUI.SetCraftingStation(this);
            craftingUI.SetVisible(true);
            isOpen = true;
        }
    }

    protected override void OnClose(Player triggerPlayer) {
        craftingInventory = null;
        craftingUI.SetVisible(false);
        isOpen = false;
    }

    public Inventory GetInventory() {
        return craftingInventory;
    }

    public int GetNumRecipes() {
        return craftableItems.Length;
    }
}
