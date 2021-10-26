using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    private GameObject dungeon;
    private TimeManager dungeonRemain;

    private void Start()
    {
        dungeonRemain = GameObject.Find("GameManager").GetComponent<TimeManager>();
        dungeon = GameObject.Find("DungeonScene");
    }
    void Update()
    {
        if (dungeonRemain.dungeonRemaining == 1)
        {
            dungeon.GetComponent<SceneChangeConditional>().allowChange = true;
        }
        else
        {
            dungeon.GetComponent<SceneChangeConditional>().allowChange = false;
        }
    }
}
