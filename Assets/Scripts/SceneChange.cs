using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public string[] sceneNames;
    public SceneLoader sceneLoader;

    private void Awake()
    {
        //Find the SceneLoader Object
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            //Teleport the player
            GameManager.instance.SaveState();
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            sceneLoader.TransitionLevel(sceneName);
        }
    }
}
