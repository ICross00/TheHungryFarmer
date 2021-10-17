using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : Interactable
{
    public CraftingRecipe[] craftableItems; //An array of all recipes that can be crafted at this station

    protected override void OnInteract(Player triggerPlayer) {
        foreach(CraftingRecipe recipe in craftableItems)
            if(recipe.CheckCanCraft(triggerPlayer.GetInventory()))
                Debug.Log(recipe);
    }
}
