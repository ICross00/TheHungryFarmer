using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlantedCrop : MonoBehaviour
{
    public ItemTemplate cropTemplate;

    private const float GROWTH_CHANCE = 0.1f; //The probability at each growth check that the crop will grow, where 0.0 = never and 1.0 = certain
    private const float GROWTH_CHECK_TIME = 0.33f; //The number of seconds that must pass before the crop attempts to grow to the next stage
    private const int MAX_GROWTH_STAGE = 4;

    private float checkAdvanceGrowth = 0.0f; //Timer variable to keep track of growth progress
    private int growthStage = 1;

    private SpriteRenderer spriteRenderer;
    private Sprite[] cropSprites;
    private Grid plantableGrid;

    void Start() {
        //Locate the array of sprites associated with crop growth stages
        SpriteListDictionary cropDict = Resources.Load<SpriteListDictionary>("Prefabs/Crop Sprite Dictionary");
        //Get the key associated with the appropriate growth sprites from the crop's name
        string cropType = cropTemplate.GetTagValue("crop_sprites");
        cropSprites = cropDict.GetSpriteList(cropType);

        //Set initial sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cropSprites[0];

        //Schedule next growth check
        checkAdvanceGrowth = Time.time + GROWTH_CHECK_TIME;

        //Find plantable grid
        plantableGrid = GetActiveGrid();
        this.transform.position = SnapPositionToGrid(this.transform.position);

    }

    /*
    Advances the crop to the next growth stage. If the growth stage
    is already at maximum, this function will do nothing.
    */
    private void AdvanceGrowthStage() {
        if(growthStage == MAX_GROWTH_STAGE) {
            return;
        }

        growthStage++; //Advance stage and update sprite
        spriteRenderer.sprite = cropSprites[growthStage - 1];
    }

    /*
    Harvests the crop. This will destroy the crop's GameObject and spawn a random amount of the crop in the world as a Collectable
    */
    public void HarvestCrop() {
        //Only yield any items if the crop was fully grown
        if(growthStage < MAX_GROWTH_STAGE) {
            int numSpawnedCrops = Random.Range(2, 6);
            Collectable spawnedItem = Collectable.Spawn(transform.position, cropTemplate.name, numSpawnedCrops, 1.5f);
            spawnedItem.ApplyRandomForce(8.0f);
        }

        Destroy(this.gameObject);
    }

    //Draw debug text
    void OnDrawGizmos() {
        Handles.Label(transform.position, "Growth Stage: " + growthStage.ToString());
    }

    // Update is called once per frame
    void Update() {
        if(Time.time > checkAdvanceGrowth) {
            float growthRoll = Random.Range(0.0f, 1.0f);
            if(growthRoll < GROWTH_CHANCE) {
                AdvanceGrowthStage();
            }

            //Schedule next growth check
            checkAdvanceGrowth = Time.time + GROWTH_CHECK_TIME;
        }
    }

    /*
    Returns the currently active Grid object
    */
    public static Grid GetActiveGrid() {
        return GameObject.Find("Grid").GetComponent<Grid>();
    }

    /*
    Snaps a vector 3 to the plantable grid
    @param worldPosition The position to snap to the grid
    @return The position snapped to the grid
    */
    private static Vector3 SnapPositionToGrid(Vector3 worldPosition) {
        Grid activeGrid = GetActiveGrid();
        Vector3Int gridPosition = activeGrid.WorldToCell(worldPosition);
        return activeGrid.CellToWorld(gridPosition);
    }

    /*
    Checks if a crop can be planted at the provided position
    @param The grid to check on
    @param position The position to test
    */
    public static bool CanPlant(Vector3 position) {
        Vector3 gridPosition = SnapPositionToGrid(position);
        Collider2D[] results = Physics2D.OverlapCircleAll(position, 0.9f, 1<<7);
        
        if(results.Length > 0) {
        foreach(Collider2D other in results) {
            Debug.Log(other.transform.gameObject);
        }
        }

        //Return true if there were 0 crops at this cell
        return results.Length == 0;
    }
}
