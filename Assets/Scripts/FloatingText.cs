using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public bool active;
    public GameObject go;
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    //Tells when to show text
    public void Show()
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
    }

    //Tells when to hide text
    public void Hide()
    {
        active = false;
        go.SetActive(active);
    }

    //Shows when to hide text after it has been called
    public void UpdateFloatingText()
    {
        if (!active)
            return;

        if (Time.time - lastShown > duration)
            Hide();

        go.transform.position += motion * Time.deltaTime;
    }
}
