using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour_FishingRod : ItemBehaviour
{
    public override void OnItemUsed(Item item, Player player, Inventory inventory) {

    }

    public override void OnItemEquipped(Item item, Player player, Inventory inventory) {
        //fishingRod
        GameObject rodPrefab = Resources.Load<GameObject>("Prefabs/FishingRod"); //TODO: Get the prefab name from a tag value of the sword item
        GameObject rod = GameObject.Instantiate(rodPrefab, player.transform.position, Quaternion.identity, player.transform);
        rod.name = "Equipped Fishing Rod";
    }

    public override void OnItemUnequipped(Item item, Player player, Inventory inventory) 
    {
        Transform rodInstance = player.transform.Find("Equipped Fishing Rod");
        if(rodInstance != null)
            Object.Destroy(rodInstance.gameObject);
    }

}
