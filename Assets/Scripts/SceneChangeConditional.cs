using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeConditional : SceneChange
{
    public bool allowChange;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && allowChange)
        {
            //Teleport the player
            GameManager.instance.SaveState();
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            sceneLoader.TransitionLevel(sceneName);
        }
    }
}
