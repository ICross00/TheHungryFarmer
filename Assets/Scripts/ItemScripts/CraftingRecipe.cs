using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class representing a crafting recipe
[Serializable]
[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Items/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Serializable]
    public struct CraftingIngredient { //Struct to store an item template and the amount. An entire instance of an item is not needed for the crafting recipe
        public ItemTemplate template;
        public int amount;
    }

    public ItemTemplate craftingOutput;
    public int outputAmount = 1;
    public CraftingIngredient[] craftingIngredients;

    /*
    Checks if an inventory contains the necessary items to craft this recipe
    @param inventory The inventory that contains the crafting ingredients
    @param consume If set to true, the ingredients will be consumed from the inventory if they exist
    @return True if the inventory has enough resources to craft the item, false if not
    */
    public bool CheckCanCraft(Inventory inventory, bool consume=false) {
        foreach(CraftingIngredient ingredient in craftingIngredients) {
            if(!inventory.ContainsItem(ingredient.template, ingredient.amount)) {
                return false;
            } else {
                if(consume)
                    inventory.RemoveItem(ingredient.template, ingredient.amount);
            }
        }

        return true;
    }

    /*
    Crafts an item by consuming ingredient items in the provided inventory and adding the
    output item to the inventory
    @param outputInv The inventory to consume resources from and add the output item into
    @return True if the item was crafted successfully, false if not
    */
    public bool Craft(Inventory outputInv) {
        if(CheckCanCraft(outputInv, true)) {
            //Create the item and add it to the inventory
            Item craftedItem = new Item { itemTemplate = craftingOutput, amount = outputAmount }; 
            outputInv.AddItem(craftedItem);
            return true;
        }

        return false;
    }
}