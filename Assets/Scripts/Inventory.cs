using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Item[] item;//Array size needs to be decalred properly. Left blank for now until item count is decided.
    int size;

    void AddItem()
    {

    }

    Item RemoveItem()
    {
        return new Item();//Must return an item after items have been declared and can be returned.
    }

    void UpdateInventory()
    {

    }
}
