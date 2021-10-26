using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour_HealingPotion : ItemBehaviour
{
    public override void OnItemUsed(Item item, Player player, Inventory inventory)
    {
        //Maxes the player health
        player.hitPoint = player.maxHitPoint;

        //Consume the item
        item.amount--;
    }
}
