using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBehaviour
{
    /*This function will be called by the Player class whenever the item is equipped.
    This does not need to be overridden.
    @param item The Item instance that was used
    @param player The Player instance that used this item
    @param inventory The inventory that this item belonged to
    */
    public virtual void OnItemEquipped(Item item, Player player, Inventory inventory) {}

    /*This function will be called by the Player class whenever the item is unequipped.
    This does not need to be overridden.
    @param item The Item instance that was used
    @param player The Player instance that used this item
    @param inventory The inventory that this item belonged to
    */

    public virtual void OnItemUnequipped(Item item, Player player, Inventory inventory) {}

    /*This function will be called by the Item class whenever the item is used by a player
    @param item The Item instance that was used
    @param player The Player instance that used this item
    @param inventory The inventory that this item belonged to
    */
    public abstract void OnItemUsed(Item item, Player player, Inventory inventory);
}
