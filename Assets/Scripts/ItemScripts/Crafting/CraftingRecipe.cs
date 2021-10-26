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

        public override string ToString() {
            return template.itemName + " x" + amount;
        }
    }

    public ItemTemplate craftingOutput;
    public int outputAmount = 1;
    public CraftingIngredient[] craftingIngredients;

    /*
    Checks if an inventory contains the necessary items to craft this recipe
    @param inventory The inventory that contains the crafting ingredients
    @return True if the inventory has enough resources to craft the item, false if not
    */
    public bool CheckCanCraft(Inventory inventory, bool consume=false) {
        foreach(CraftingIngredient ingredient in craftingIngredients) {
            if(!inventory.ContainsItem(ingredient.template, ingredient.amount) | inventory.IsFull())
                return false;
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
        if(CheckCanCraft(outputInv)) {
            //Consume the items
            foreach(CraftingIngredient ingredient in craftingIngredients)
                outputInv.RemoveItem(ingredient.template, ingredient.amount);

            //Create the item and add it to the inventory
            Item craftedItem = new Item { itemTemplate = craftingOutput, amount = outputAmount }; 
            outputInv.AddItem(craftedItem);
            return true;
        }

        return false;
    }

    /*
    Returns the recipe as a formatted string, with ingredients highlighted in red
    if they do not exist in the inventory
    @param inventory The inventory to check for ingredients in
    */
    public string FormatIngredientString(Inventory inventory) {
        string s = "";
        foreach(CraftingIngredient ingredient in craftingIngredients) {
            string c = inventory.ContainsItem(ingredient.template, ingredient.amount)  ? "53E079" : "BF4D4D";
            s += "<color=#"+c+">"+ingredient + "</color>\n";
        }

        return s.Substring(0,s.Length-1);
    }
}