using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnimals : MonoBehaviour
{
    private GameObject animal;

    public GameObject SpawnPoint;
    private List<GameObject> SpawnPointsChild = new List<GameObject>();
    GameObject Animal;
    Transform player;
    float distance;
    public int numberAnimalsWanted;
    public int wantedDistance;
    private int currentAnimalCount;

    GameObject animalSpawn;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        SpawnPoint = GameObject.Find("AnimalSpawns");

        foreach (Transform child in SpawnPoint.transform)
        {
            if (child.tag == "Spawns")
            {
                SpawnPointsChild.Add(child.gameObject);
            }
        }

        for (int n = 0; n < SpawnPointsChild.Count; n++)
        {
            AnimalSelect(n);
        }
    }

    private void Update()
    {
        currentAnimalCount = GameObject.FindGameObjectsWithTag("Animal").Length;

        if (currentAnimalCount < numberAnimalsWanted)
        {
            SpawnAdditionalAnimal();
        }
    }

    private void AnimalSelect(int n)
    {
        int randNum = Random.Range(1, 3);

        if (randNum == 1)
        {
            animal = Resources.Load<GameObject>("Prefabs/Wilderness/Pig");
            Animal = Instantiate(animal, SpawnPointsChild[n].transform.position, Quaternion.identity);
        }
        else if (randNum == 2)
        {
            animal = Resources.Load<GameObject>("Prefabs/Wilderness/Rabbit");
            Animal = Instantiate(animal, SpawnPointsChild[n].transform.position, Quaternion.identity);
        }
    }

    void SpawnAdditionalAnimal()
    {
        //This will spawn an enemy outside of camera range when an enemy is called.
        for (int n = 0; n < SpawnPointsChild.Count; n++)
        {
            distance = Vector3.Distance(transform.position, SpawnPointsChild[n].transform.position);

            if (distance > wantedDistance)
            {
                AnimalSelect(n);
                break;
            }
        }
    }
}
