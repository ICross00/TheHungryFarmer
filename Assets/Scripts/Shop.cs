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

    private Player playerCustomer;
    private Inventory playerInventory;
    private UI_Inventory playerInventoryUI;

    public void OpenShop(Player customer) {
        shopUI.SetInventory(shopInventory);
        playerCustomer = customer;
        playerInventory = customer.GetComponent<Inventory>();
        playerInventoryUI = customer.inventoryUI;
    }

    public void SellItem(Item item, int sellAmount) {
        item.amount -= sellAmount;
        playerInventory.AddItem(new Item { item = goldItem.item, amount = sellAmount });
    }
}
