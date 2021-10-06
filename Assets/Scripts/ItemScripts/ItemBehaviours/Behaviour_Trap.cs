using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour_Trap : ItemBehaviour
{
    public override void OnItemUsed(Item item, Player player, Inventory inventory) {
        GameObject pfTrap = Resources.Load<GameObject>("Prefabs/pfTrap");
        Transform trapGrid = GameObject.Find("Grid").transform;

        GameObject trap = GameObject.Instantiate(pfTrap, player.transform.position, Quaternion.identity, trapGrid);

        //Consume the item
        inventory.RemoveItemSingle(item);
    }
}
