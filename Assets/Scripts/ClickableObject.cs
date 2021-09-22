using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

//Custom button that allows both left and right clicks
public class ClickableObject: MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public int index = 0;

    public UnityEvent<int> onLeft;
    public UnityEvent<int> onRight;

    public UnityEvent<int> onHover;
    public UnityEvent<int> onExit;
    //The image that will highlight this clickable object when it is moused over
    private Image image;

    void Start() {
        image = GetComponent<Image>();
        SetImageAlpha(0f);
    }

    //Detect if the cursor clicks on the gameobject
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeft.Invoke(index);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRight.Invoke(index);
        }
    }

    //Detect if the cursor starts to pass over the gameobject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        SetImageAlpha(0.44f);
        onHover.Invoke(index);
    }

    //Detect when cursor leaves the gameobject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        SetImageAlpha(0f);
        onExit.Invoke(index);
    }

    /*
    Sets the alpha component of the ClickableObject's Image
    @param alpha The new alpha to set, ranging between 0 and 1
    */
    public void SetImageAlpha(float alpha) {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    public void RemoveAllListeners() {
        onLeft.RemoveAllListeners();
        onRight.RemoveAllListeners();
    }
}
