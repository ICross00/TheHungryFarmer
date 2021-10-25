using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSpawns : MonoBehaviour
{
    private GameObject enemy;

    public GameObject SpawnPoint;
    private List<GameObject> SpawnPointsChild = new List<GameObject>();

    GameObject enemySpawn;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPoint = GameObject.Find("SpawnPoints");

        foreach (Transform child in SpawnPoint.transform)
        {
            if (child.tag == "Spawns")
            {
                SpawnPointsChild.Add(child.gameObject);
            }
        }

        for (int n = 0; n < SpawnPointsChild.Count; n++)
        {
            EnemySelect(n);
        }
    }

    private void EnemySelect(int n)
    {
        int randNum = Random.Range(1, 5);

        if (randNum == 1)
        {
            enemy = Resources.Load<GameObject>("Prefabs/SmallEnemy");
            enemySpawn = Instantiate(enemy, SpawnPointsChild[n].transform.position, Quaternion.identity);
        }
        else if (randNum == 2)
        {
            enemy = Resources.Load<GameObject>("Prefabs/SlimeEnemy");
            enemySpawn = Instantiate(enemy, SpawnPointsChild[n].transform.position, Quaternion.identity);
        }
        else if (randNum == 3)
        {
            enemy = Resources.Load<GameObject>("Prefabs/BatEnemy");
            enemySpawn = Instantiate(enemy, SpawnPointsChild[n].transform.position, Quaternion.identity);
        }
    }
}
