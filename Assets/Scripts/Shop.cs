using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Class representing a shop. This must be given a UI_Inventory object on which it will display the shop inventory.
    When a player interacts with something that causes the shop to open, the player object should be received using the
    OpenShop function.
*/
public class Shop : MonoBehaviour
{
    //The items that this shop will sell
    public Inventory shopInventory;
    public UI_Inventory shopUI;
    public Item goldItem;

    public Player playerCustomer;
    private Inventory playerInventory;
    private UI_Inventory playerInventoryUI;

    void Awake() {
        shopUI.SetInventory(shopInventory);

        //Set up callback functions for when an item is sold

        shopUI.onButtonRightClicked.AddListener((int slotIndex) => {
            Item soldItem = shopInventory.GetItemList()[slotIndex];
            SellItem(soldItem);
        });

        OpenShop(playerCustomer);
    }

    /**
    Displays a shop screen to a player, who is treated as the shop's customer until the shop is closed
    @param customer The player to display t he shop to
    */
    public void OpenShop(Player customer) {
        playerCustomer = customer;
        playerInventory = customer.GetComponent<Inventory>();
        playerInventoryUI = customer.inventoryUI;

        shopUI.SetOwner(customer);
        shopUI.SetInventory(shopInventory);
    }

    /**
    Sells an item by removing the provided item from the customer's inventory, and adding gold to the customer's inventory
    the item that was sold and in what amount
    @param item The item to sell, including the amount
    */
    public void SellItem(Item item) {
        if(playerInventory.RemoveItem(item)) {
            playerInventory.AddItem(new Item { item = goldItem.item, amount = item.GetSellPrice() * item.amount });
        }
    }
}
