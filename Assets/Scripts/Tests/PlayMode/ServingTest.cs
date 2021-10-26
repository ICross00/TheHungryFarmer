using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class ServingTest
{
    //Declare variables for use in the tests
    GameObject inventoryObject;
    GameObject experienceObject;
    Inventory inventory;
    CraftingRecipe foodRecipe;
    XpManager xpManager;
    ItemTemplate rawPorkTemplate;

    [SetUp]
    public void Setup()
    {
        inventoryObject = new GameObject();
        experienceObject = new GameObject();

        inventory = inventoryObject.AddComponent<Inventory>();
        foodRecipe = Resources.Load<CraftingRecipe>("Items/Recipes/Bacon Recipe");
        rawPorkTemplate = Resources.Load<ItemTemplate>("Items/Raw_Pork");
        xpManager = experienceObject.AddComponent<XpManager>();
        xpManager.currentXP = 0;
    }

    public void TearDown()
    {

    }

    public IEnumerator TestXPForCooking()
    {
        //Add raw pork to the inventory for crafting.
        inventory.AddItem(Item.CreateItem("Raw_Pork", 1));

        //Craft the pork into the bacon.
        foodRecipe.Craft(inventory);

        //bool hasBacon = inventory.ContainsItem

        //Check if the player's experience has increased.
        Assert.AreNotEqual(0, xpManager.currentXP);

        yield return null;
    }
}
