using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon2 : MonoBehaviour
{
    Gaurdian gaurdian;
    public GameObject dungeon2;

    private void Start()
    {
        dungeon2 = GameObject.Find("Dungeon2SceneChange");
        gaurdian = GameObject.Find("GaurdianOne").GetComponent<Gaurdian>();
    }

    private void Update()
    {
        if (gaurdian.hitPoint <= 0)
        {
            dungeon2.GetComponent<SceneChangeConditional>().allowChange = true;
        }
        else
        {
            dungeon2.GetComponent<SceneChangeConditional>().allowChange = false;
        }
    }
}
