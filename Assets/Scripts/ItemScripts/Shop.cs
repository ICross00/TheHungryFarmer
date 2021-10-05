using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Class representing a shop. This must be given a UI_Inventory object on which it will display the shop inventory.
    When a player interacts with something that causes the shop to open, the player object should be received using the
    OpenShop function.

    For the purpose of this class, the player who is currently interacting with the shop is referred to as the customer.
*/
public class Shop : Interactable
{
    //The items that this shop will sell
    private Inventory shopInventory;
    public UI_Inventory shopUI;

    private Player playerCustomer;
    private Inventory playerInventory;
    private bool isShopOpen = false;

    void Awake() {
        shopInventory = GetComponent<Inventory>();
        shopUI.SetInventory(shopInventory);

        //Set up callback functions for when an item is bought or sold

        shopUI.onButtonRightClicked.AddListener((int slotIndex) => {
            Item soldItem = shopInventory.GetItemList()[slotIndex];
            SellItem(soldItem);
        });

        shopUI.onButtonLeftClicked.AddListener((int slotIndex) => {
            Item purchasedItem = shopInventory.GetItemList()[slotIndex];
            if(playerCustomer != null) {
                BuyItem(purchasedItem);
            }
        });
    }

    /**
    Displays a shop screen to a player, who is treated as the shop's customer until the shop is closed
    @param customer The player to display t he shop to
    */
    public void OpenShop(Player customer) {
        playerCustomer = customer;
        playerInventory = customer.GetInventory();

        shopUI.SetInventory(shopInventory);
        shopUI.SetVisible(true);

        isShopOpen = true;
    }

    /**
    Closes the shop and removes the customer
    */
    protected override void OnClose(Player customer) {
        playerCustomer = null;
        playerInventory = null;

        shopUI.SetVisible(false);
        isShopOpen = false;
    }

    protected override void OnInteract(Player triggerPlayer) {
        if(isShopOpen) {
            Close(triggerPlayer);
        } else {
            OpenShop(triggerPlayer);
        }
    }

    /**
    Sells an item by removing the provided item from the customer's inventory, and adding gold to the customer's inventory
    the item that was sold and in what amount
    @param soldItem The item to sell, including the amount
    @return True if the player could sell the selected item, false otherwise
    */
    public bool SellItem(Item soldItem) {
        if(playerInventory.RemoveItem(soldItem)) {
            int totalSaleAmount = soldItem.GetTotalSellPrice();
            playerCustomer.ChangeGold(totalSaleAmount);
            return true;
        }
        return false;
    }

    /**
    Purchases an item for the customer by adding a copy of it to their inventory and deducting the appropriate amount of gold
    If the item
    @param purchasedItem The item to purchase, including the amount
    @return True if the player could buy the selected item, false otherwise
    */
    public bool BuyItem(Item purchasedItem) {
        int cost = purchasedItem.GetTotalSellPrice();
        if(playerCustomer.GetGold() >= cost) {
            playerInventory.AddItem(Item.CopyItem(purchasedItem));
            playerCustomer.ChangeGold(-cost); //Deduct gold

            return true;
        }

        return false;
    }
}
