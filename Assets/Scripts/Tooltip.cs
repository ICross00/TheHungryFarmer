using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private static readonly float prefabSize = 0.2f;
    public float padding = 40.0f;
    private TextMeshProUGUI tooltipText;
    private Image background;

    void Awake() {
        tooltipText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        //tooltipText.anchor = AnchorPositions.BottomRight;
        background = transform.Find("Background").GetComponent<Image>();

        GameObject inventory = transform.parent.gameObject;
    }

    public void SetText(string text) {
        tooltipText.text = text;
        //Calculate size of tooltip background based on the provided text
        Vector2 textSize = tooltipText.GetPreferredValues();
        textSize = textSize + new Vector2(padding, padding * 0.5f); //Apply padding
        textSize = textSize * prefabSize; //Scale by the size of the prefab's transform

        background.rectTransform.sizeDelta = textSize;
    }

    /**
    Shows or hides the tooltip from being visible
    */
    public void ShowTooltip(bool show) {
        transform.SetAsLastSibling();
        gameObject.SetActive(show);
    }

    private void FixedUpdate() {
        //Convert the cursor position to a screen space coordinate that can be used to position the tooltip within the inventory frame
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out localPoint);
        transform.localPosition = localPoint;
    }
}
