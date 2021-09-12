using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField]
    private Inventory m_inventory; //The inventory object that this will display a UI for
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private bool isVisible;

    private void Start() {
        itemSlotContainer = transform.Find("InventoryImage");
        itemSlotTemplate = itemSlotContainer.Find("SlotTemplate");
        m_inventory.OnItemListChanged += UI_OnItemListChanged;
        isVisible = false;

        Refresh();
    }
    //Refresh when the inventory data structure is changed
    private void UI_OnItemListChanged(object sender, System.EventArgs e) {
        Refresh();
    }

    /*
    Toggles the inventory gameobject between visible and not visible
    */
    public void ToggleVisible()  {
        SetVisible(!isVisible);
    }

    /*
    Makes the inventory gameobject appear visible
    If visible is true, the gameobject will appear. If visible is false, the game object will be hidden
    */
    private void SetVisible(bool visible) {
        isVisible = visible;
        gameObject.SetActive(isVisible);
        Refresh();
    }

    /*
    Redraws the inventory by destroying all the slot gameobjects and recreating them from the current inventory state
    */
    private void Refresh() {
        //Destroy old gameobjects
        foreach(Transform child in itemSlotContainer) {
            if(child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float cellSize = 21f;
        float cellMargin = 0f;

        foreach (Item item in m_inventory.GetItemList()) {
            RectTransform slotTf = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            slotTf.gameObject.SetActive(true);
            RectTransform templateTf = itemSlotTemplate.GetComponent<RectTransform>();
            slotTf.anchoredPosition = templateTf.anchoredPosition + new Vector2(x * cellSize + cellMargin, y * cellSize + cellMargin);

            Image image = slotTf.Find("Item").GetComponent<Image>();
            image.sprite = item.GetSprite();

            Text amountText = slotTf.Find("AmountText").GetComponent<Text>();
            amountText.text = item.amount.ToString();
            x++;
            if(x > 8) {
                x = 0;
                y--;
            }
        }

    }


}
