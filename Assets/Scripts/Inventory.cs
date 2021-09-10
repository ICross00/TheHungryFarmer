using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory
{
    private List<Item> items;

    public Inventory() {
        items = new List<Item>();
        Debug.Log("Inventory initialized");

        AddItem(new Item(Item.ItemType.Heart, 1));
        AddItem(new Item(Item.ItemType.Star, 1));
        //AddItem(new Item(Item.ItemType.Pot, 1));
        Debug.Log(items.Count);
    }

    public List<Item> GetItemList() {
        return items;
    }

    public void AddItem(Item item) {
        items.Add(item);
    }
}
