using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    private void Awake() {
        itemSlotContainer = transform.Find("InventoryImage");
        itemSlotTemplate = itemSlotContainer.Find("SlotTemplate");
        SetInventory(GameManager.instance.GetInventory());
    }

    public void SetInventory(Inventory inventory) {
        this.inventory = inventory;
        Refresh();
    }

    private void Refresh() {
        int x = 0;
        int y = 0;
        float cellSize = 22f;
        float cellMargin = 0f;

        foreach (Item item in inventory.GetItemList()) {
            RectTransform slotTf = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            slotTf.gameObject.SetActive(true);
            RectTransform templateTf = itemSlotTemplate.GetComponent<RectTransform>();
            slotTf.anchoredPosition = templateTf.anchoredPosition + new Vector2(x * cellSize + cellMargin, y * cellSize + cellMargin);

            Image image = slotTf.Find("Item").GetComponent<Image>();
            image.sprite = item.GetSprite();

            x++;
            if(x > 9) {
                x = 0;
                y++;
            }
        }

    }


}
