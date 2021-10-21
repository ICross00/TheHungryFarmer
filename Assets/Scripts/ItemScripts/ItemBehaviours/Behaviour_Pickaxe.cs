using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour_Pickaxe : ItemBehaviour
{
    public override void OnItemUsed(Item item, Player player, Inventory inventory) {
        PlayerAnimator animator = player.GetComponent<PlayerAnimator>();
    }

    public override void OnItemEquipped(Item item, Player player, Inventory inventory) {
        //Instantiate the sword prefab as a child of the player
        GameObject swordPrefab = Resources.Load<GameObject>("Prefabs/" + item.GetItemTagValue("prefab_name")); //TODO: Get the prefab name from a tag value of the sword item
        GameObject sword = GameObject.Instantiate(swordPrefab, player.transform.position, Quaternion.identity, player.transform);
        sword.name = "Equipped Sword";
    }

    public override void OnItemUnequipped(Item item, Player player, Inventory inventory) {
        //Destroy the sword prefab
        Transform swordInstance = player.transform.Find("Equipped Sword");
        if(swordInstance != null)
            Object.Destroy(swordInstance.gameObject);
    }

}
