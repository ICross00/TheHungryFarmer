using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour_Trap : ItemBehaviour
{
    public override void OnItemUsed(Item item, Player player, Inventory inventory) {
        GameObject pfTrap = Resources.Load<GameObject>("Prefabs/pfTrap");
        Transform trapGrid = GameObject.Find("Grid").transform;
        int tier = int.Parse(item.GetItemTagValue("tier"));

        GameObject trap = GameObject.Instantiate(pfTrap, player.transform.position, Quaternion.identity);
        trap.GetComponent<Trap>().SetTier(tier);
        trap.GetComponent<Trap>().SetSprite(item.GetSprite());

        //Consume the item
        item.amount--;
    }
}
