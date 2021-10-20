using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour_FishingRod : ItemBehaviour
{
    public override void OnItemUsed(Item item, Player player, Inventory inventory) {

    }

    public override void OnItemEquipped(Item item, Player player, Inventory inventory) {
        //fishingRod
        GameObject frodPrefab = Resources.Load<GameObject>("Prefabs/FishingRod"); //TODO: Get the prefab name from a tag value of the sword item
        GameObject frod = GameObject.Instantiate(frodPrefab, player.transform.position, Quaternion.identity, player.transform);
        frod.name = "Equipped Fishing Rod";
    }

    public override void OnItemUnequipped(Item item, Player player, Inventory inventory) 
    {
        Transform frodInstance = player.transform.Find("Equipped Fishing Rod");
        if(frodInstance != null)
            Object.Destroy(frodInstance.gameObject);
    }

}
