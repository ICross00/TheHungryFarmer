using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CraftingTest
{
    /*
    Tests the Craft function in the CraftingRecipe class by attempting to craft 1x Trap3 using 5x Ruby and 1x Trap2
    */
    [UnityTest]
    public IEnumerator CraftHasAllItems() 
    {
        //Initialize an inventory
        var inventoryObject = new GameObject();
        PlayerInventory inventory = inventoryObject.AddComponent<PlayerInventory>();

        //Add a Trap2 and 5 rubies to the player's inventory to be used in crafting the recipe
        inventory.AddItem(Item.CreateItem("Trap2", 1));
        inventory.AddItem(Item.CreateItem("Ruby", 10));

        //Get the crafting recipe for Trap3
        var craftingRecipe = Resources.Load<CraftingRecipe>("Items/Recipes/Red Trap Recipe");

        //Craft the item
        craftingRecipe.Craft(inventory);

        //Check that there is 5 Ruby, 0 Trap2, and 1 Trap3 in the inventory
        ItemTemplate rubyTemplate = Resources.Load<ItemTemplate>("Items/Ruby");
        ItemTemplate trap2Template = Resources.Load<ItemTemplate>("Items/Trap2");
        ItemTemplate trap3Template = Resources.Load<ItemTemplate>("Items/Trap3");

        bool hasCorrectNumRuby = inventory.ContainsItem(rubyTemplate, 5, true); //True parameter means to look for exact amount
        bool hasTrap2 = inventory.ContainsItem(trap2Template, 1 , true);
        bool hasTrap3 = inventory.ContainsItem(trap3Template, 1 , true);

        Assert.AreEqual(true, hasCorrectNumRuby);
        Assert.AreEqual(false, hasTrap2);
        Assert.AreEqual(true, hasTrap3);

        yield return null;
    }

    /*
    Tests that the Craft function does not have any effect on the inventory when there is a missing item
    */
    [UnityTest]
    public IEnumerator CraftMissingItems() {
        //Initialize an inventory
        var inventoryObject = new GameObject();
        PlayerInventory inventory = inventoryObject.AddComponent<PlayerInventory>();

        //Add a Trap2 and 4 rubies to the player's inventory to be used in crafting the recipe
        inventory.AddItem(Item.CreateItem("Trap2", 1));
        inventory.AddItem(Item.CreateItem("Ruby", 4));

        //Get the crafting recipe for Trap3
        var craftingRecipe = Resources.Load<CraftingRecipe>("Items/Recipes/Red Trap Recipe");

        //Craft the item
        craftingRecipe.Craft(inventory);

        //Check that there is 4 Ruby, 1 Trap2, and 0 Trap3 in the inventory
        ItemTemplate rubyTemplate = Resources.Load<ItemTemplate>("Items/Ruby");
        ItemTemplate trap2Template = Resources.Load<ItemTemplate>("Items/Trap2");
        ItemTemplate trap3Template = Resources.Load<ItemTemplate>("Items/Trap3");

        bool hasCorrectNumRuby = inventory.ContainsItem(rubyTemplate, 4, true); //True parameter means to look for exact amount
        bool hasTrap2 = inventory.ContainsItem(trap2Template, 1 , true);
        bool hasTrap3 = inventory.ContainsItem(trap3Template, 1 , true);

        Assert.AreEqual(true, hasCorrectNumRuby);
        Assert.AreEqual(true, hasTrap2);
        Assert.AreEqual(false, hasTrap3);

        yield return null;
    }

    /*
    Tests that the Craft function does not have any effect on the inventory when there would be no space to store the output
    */
    [UnityTest]
    public IEnumerator CraftInventoryFull() {
        //Initialize an inventory
        var inventoryObject = new GameObject();
        PlayerInventory inventory = inventoryObject.AddComponent<PlayerInventory>();

        for(int i = 0; i < 25; i++) //Fill the player's inventory
            inventory.AddItem(Item.CreateItem("Sword1", 1));

        //Add crafting ingredients. The inventory is now completely full
        inventory.AddItem(Item.CreateItem("Trap2", 2));
        inventory.AddItem(Item.CreateItem("Ruby", 6));

        //Get the crafting recipe for Trap3
        var craftingRecipe = Resources.Load<CraftingRecipe>("Items/Recipes/Red Trap Recipe");

        //Craft the item
        craftingRecipe.Craft(inventory);

        //Check that there is 6 Ruby, 2 Trap2, and 0 Trap3 in the inventory. No resources consumed and no item added
        ItemTemplate rubyTemplate = Resources.Load<ItemTemplate>("Items/Ruby");
        ItemTemplate trap2Template = Resources.Load<ItemTemplate>("Items/Trap2");
        ItemTemplate trap3Template = Resources.Load<ItemTemplate>("Items/Trap3");

        bool hasCorrectNumRuby = inventory.ContainsItem(rubyTemplate, 6, true); //True parameter means to look for exact amount
        bool hasTrap2 = inventory.ContainsItem(trap2Template, 2 , true);
        bool hasTrap3 = inventory.ContainsItem(trap3Template, 1 , true);

        Assert.AreEqual(true, hasCorrectNumRuby);
        Assert.AreEqual(true, hasTrap2);
        Assert.AreEqual(false, hasTrap3);

        yield return null;
    }
}
