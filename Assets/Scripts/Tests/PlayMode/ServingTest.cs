using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class ServingTest
{
    //Declare variables for use in the tests
    GameObject inventoryObject;
    GameObject experienceObject;
    Inventory inventory;
    CraftingRecipe foodRecipe;
    XpManager xpManager;
    ItemTemplate rawPorkTemplate;
    ItemTemplate baconTemplate;

    [SetUp]
    public void Setup()
    {
        inventoryObject = new GameObject();
        experienceObject = new GameObject();

        inventory = inventoryObject.AddComponent<Inventory>();
        foodRecipe = Resources.Load<CraftingRecipe>("Items/Recipes/Bacon Recipe");
        rawPorkTemplate = Resources.Load<ItemTemplate>("Items/Raw_Pork");
        baconTemplate = Resources.Load<ItemTemplate>("Items/Cooked_Bacon");
        xpManager = experienceObject.AddComponent<XpManager>();
        xpManager.testing = true;
        xpManager.currentXP = 0;
    }

    public void TearDown()
    {

    }

    [UnityTest]
    public IEnumerator TestXPForCooking()
    {
        //Add raw pork to the inventory for crafting.
        inventory.AddItem(Item.CreateItem("Raw_Pork", 1));

        //Craft the pork into the bacon.
        foodRecipe.Craft(inventory);

        //Booleans to check if items have crafted.
        bool hasRawPork = inventory.ContainsItem(rawPorkTemplate, 1, true);
        bool hasBacon = inventory.ContainsItem(baconTemplate, 1, true);

        //Check if the player's experience has increased.
        Assert.AreNotEqual(0, xpManager.currentXP);
        Assert.AreEqual(false, hasRawPork);
        Assert.AreEqual(true, hasBacon);

        yield return null;
    }
}
