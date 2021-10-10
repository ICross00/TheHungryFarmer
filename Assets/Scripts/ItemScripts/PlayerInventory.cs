using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    public event EventHandler OnHotbarChanged;
    public static readonly int HOTBAR_MAX = UI_Inventory.ROW_SIZE;
    private List<Item> hotbar = new List<Item>();

    void Start() {
         OnHotbarChanged += Inventory_OnHotbarChanged;
    }

    /*
    Forces the hotbar to refresh
    */
    public void TriggerRefresh() {
        hotbar.RemoveAll(hotbarItem => hotbarItem.amount == 0);
        OnHotbarChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Inventory_OnHotbarChanged(object sender, System.EventArgs e) {
        
    }

    public Item GetItemFromHotbar(int slotIndex) {
        if(slotIndex >= hotbar.Count)
            return null;

        return hotbar[slotIndex];
    }

    public int GetNumHotbarItems() {
        return hotbar.Count;
    }

    public bool IsHotbarFull() {
        return hotbar.Count >= HOTBAR_MAX;
    }

    /*
    Transfers an item from the inventory to the hotbar
    @param slotIndex The index in the inventory to transfer an item from
    @return True if the item could be transferred, false if not (i.e. the hotbar is full)
    */
    public bool TransferToHotbar(int slotIndex) {
        if(hotbar.Count >= HOTBAR_MAX)
            return false;

        Item itemToHotbar = RemoveItem(slotIndex);
        hotbar.Add(itemToHotbar);

        //Notify any listeners that the hotbar changed
        OnHotbarChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /*
    Transfers an item from the hotbar to the inventory
    @param slotIndex The index in the hotbar to transfer an item from
    @return True if the item could be transferred, false if not (i.e. the inventory is full)
    */
    public bool TransferFromHotbar(int hotbarIndex) {
        Item itemToInventory = hotbar[hotbarIndex];
        hotbar.RemoveAt(hotbarIndex);

        AddItem(itemToInventory);

        //Notify any listeners that the hotbar changed
        OnHotbarChanged?.Invoke(this, EventArgs.Empty);
        return true; //TODO: Add inventory size limit
    }

    public List<Item> GetHotbarList() {
        return hotbar;
    }
}
