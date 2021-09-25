using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Basic class to persist an object between scenes, but to only have it visible on the scene it was initialized in
public class Persist : MonoBehaviour
{
    private static Dictionary<Tuple<string, string>, bool> persistSceneTargets = new Dictionary<Tuple<string, string>, bool>();
    private Tuple<string, string> key;

    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;

        key = GetDictionaryKey();
        if(persistSceneTargets.ContainsKey(key)) {
            Destroy(gameObject); //Destroy the object if it has already been cloned
        } else {
            DontDestroyOnLoad(this);
            persistSceneTargets[key] = true;
        }
    }

    private Tuple<string, string> GetDictionaryKey() {
        if(key == null) {
            return new Tuple<string, string>(gameObject.name, SceneManager.GetActiveScene().name);
        } else {
            return key;
        }
    }

    void ChangedActiveScene(Scene c, Scene n) {
        if(this == null)
            return;

        Tuple<string, string> currentKey = GetDictionaryKey();
        if(persistSceneTargets.ContainsKey(currentKey)) {
            this.gameObject.SetActive(n.name == currentKey.Item2);
        }
    }
}
