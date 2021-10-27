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
    GameObject managerObject;
    GameObject foodCounterObject;

    Inventory inventory;
    CraftingRecipe foodRecipe;
    XpManager xpManager;
    ItemTemplate rawPorkTemplate;
    ItemTemplate baconTemplate;

    FoodCounter foodCounter;
    GameManager gameManager;
    TimeManager timeManager;

    [SetUp]
    public void Setup()
    {
        inventoryObject = new GameObject();
        experienceObject = new GameObject();
        managerObject = new GameObject();
        foodCounterObject = new GameObject();

        inventory = inventoryObject.AddComponent<Inventory>();
        foodRecipe = Resources.Load<CraftingRecipe>("Items/Recipes/Bacon Recipe");
        rawPorkTemplate = Resources.Load<ItemTemplate>("Items/Raw_Pork");
        baconTemplate = Resources.Load<ItemTemplate>("Items/Cooked_Bacon");
        xpManager = experienceObject.AddComponent<XpManager>();
        xpManager.testing = true;
        xpManager.currentXP = 0;

        foodCounter = foodCounterObject.AddComponent<FoodCounter>();
        gameManager = managerObject.AddComponent<GameManager>();
        timeManager = managerObject.AddComponent<TimeManager>();
        gameManager.gold = 0;
        foodCounter.test = true;
        gameManager.test = true;
        foodCounter.timeManager = timeManager;
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

    [UnityTest]
    public IEnumerator TestGoldForServing()
    {
        //Set the food counter to have a food item stored.
        foodCounter.SetStoredItem(Item.CreateItem("Cooked_Bacon", 1));

        //Save value before going to the next day.
        int goldBefore = gameManager.GetGold();

        //Manually add money in place of player interaction.
        foodCounter.AddMoneyEarned();

        //Go to the next day.
        gameManager.NextDayOperationsTest();

        //Save value of gold after.
        int goldAfter = gameManager.GetGold();

        //Gold should not be equal if it has been earned.
        Assert.AreNotEqual(goldBefore, goldAfter);

        yield return null;
    }
}
