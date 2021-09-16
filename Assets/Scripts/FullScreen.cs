using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreen : MonoBehaviour
{
    public void Full_Screen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        Debug.Log("Full Screen is " + isFullScreen);
    }
}
