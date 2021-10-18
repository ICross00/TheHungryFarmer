using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingTrigger : MonoBehaviour
{
    private FishingGame game;
    private GameObject canvas;

    private void Awake()
    {
        game = GetComponentInChildren<FishingGame>();
        canvas = GameObject.Find("FishingCanvas Variant");
        canvas.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collide)
    {
        Debug.Log(game);
        if (collide.name == "Player")
        {
            canvas.SetActive(true);
            game.Start_Game();
        }
    }
}
