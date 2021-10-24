using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CraftingTest
{
    private GameObject inventoryObject;     //An object to hold the inventory
    private PlayerInventory inventory;      //The inventory that will be used for crafting

    private CraftingRecipe testRecipe;      //The recipe that will be crafted and used for testing
    private ItemTemplate rubyTemplate;      //Item templates for crafting ingredients
    private ItemTemplate trap2Template;
    private ItemTemplate trap3Template;

    [SetUp] //Assign values to all of the private fields before running tests
    public void SetUp() {
        //Initialize an inventory
        inventoryObject = new GameObject();
        inventory = inventoryObject.AddComponent<PlayerInventory>();

        //Get the crafting recipe for Trap3
        testRecipe = Resources.Load<CraftingRecipe>("Items/Recipes/Red Trap Recipe");

        //Get the crafting ingredients that will be used for testing
        rubyTemplate = Resources.Load<ItemTemplate>("Items/Ruby");
        trap2Template = Resources.Load<ItemTemplate>("Items/Trap2");
        trap3Template = Resources.Load<ItemTemplate>("Items/Trap3");
    }

    [TearDown] //Unassign all of the private fields
    public void TearDown() {
        Object.Destroy(inventoryObject);
        inventory = null;
        testRecipe = null;
        rubyTemplate = trap2Template = trap3Template = null;
    }

    /*
    Tests the Craft function in the CraftingRecipe class by attempting to craft 1x Trap3 using 5x Ruby and 1x Trap2
    */
    [UnityTest]
    public IEnumerator CraftHasAllItems() {
        //Add a Trap2 and 5 rubies to the player's inventory to be used in crafting the recipe
        inventory.AddItem(Item.CreateItem("Trap2", 1));
        inventory.AddItem(Item.CreateItem("Ruby", 10));

        //Craft the item
        testRecipe.Craft(inventory);

        //Check that there is 5 Ruby, 0 Trap2, and 1 Trap3 in the inventory
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
        //Add a Trap2 and 4 rubies to the player's inventory to be used in crafting the recipe
        inventory.AddItem(Item.CreateItem("Trap2", 1));
        inventory.AddItem(Item.CreateItem("Ruby", 4));

        //Craft the item
        testRecipe.Craft(inventory);

        //Check that there is 4 Ruby, 1 Trap2, and 0 Trap3 in the inventory
        bool hasCorrectNumRuby = inventory.ContainsItem(rubyTemplate, 4, true);
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
        for(int i = 0; i < 25; i++) //Fill the player's inventory
            inventory.AddItem(Item.CreateItem("Sword1", 1));

        //Add crafting ingredients. The inventory is now completely full
        inventory.AddItem(Item.CreateItem("Trap2", 2));
        inventory.AddItem(Item.CreateItem("Ruby", 6));

        //Craft the item
        testRecipe.Craft(inventory);

        //Check that there is 6 Ruby, 2 Trap2, and 0 Trap3 in the inventory. No resources consumed and no item added
        bool hasCorrectNumRuby = inventory.ContainsItem(rubyTemplate, 6, true);
        bool hasTrap2 = inventory.ContainsItem(trap2Template, 2 , true);
        bool hasTrap3 = inventory.ContainsItem(trap3Template, 1 , true);

        Assert.AreEqual(true, hasCorrectNumRuby);
        Assert.AreEqual(true, hasTrap2);
        Assert.AreEqual(false, hasTrap3);

        yield return null;
    }
}
