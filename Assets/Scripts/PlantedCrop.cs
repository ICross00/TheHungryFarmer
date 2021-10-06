using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlantedCrop : RandomEvent
{
    public ItemTemplate cropTemplate;
    private int maxGrowthStage;
    private int growthStage = 1;

    private SpriteRenderer spriteRenderer;
    private Sprite[] cropSprites;
    private Grid plantableGrid;
    private PlantableArea plantableArea;

    void Start() {
        gameObject.tag = "Crop";
        //Set growth probabilities
        probability = 0.1f;
        period = 0.33f;

        //Locate the array of sprites associated with crop growth stages
        SpriteListDictionary cropDict = Resources.Load<SpriteListDictionary>("Prefabs/Crop Sprite Dictionary");

        //Get the key associated with the appropriate crop sprites
        string cropType = cropTemplate.GetTagValue("crop_sprites");
        cropSprites = cropDict.GetSpriteList(cropType);
        maxGrowthStage = cropSprites.Length;

        //Set initial sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cropSprites[0];

        //Find plantable grid
        plantableGrid = GetActiveGrid();
        this.transform.position = SnapPositionToGrid(this.transform.position);

        //Find plantable area
        plantableArea = GameObject.Find("PlantableArea").GetComponent<PlantableArea>();
    }

    /*
    Advances the crop to the next growth stage. If the growth stage
    is already at maximum, this function will do nothing.
    */
    protected override void OnRandomEventTriggered() {
        if(growthStage == maxGrowthStage) {
            return;
        }

        growthStage++; //Advance stage and update sprite
        spriteRenderer.sprite = cropSprites[growthStage - 1];
    }

    /*
    Harvests the crop. This will destroy the crop's GameObject and spawn a random amount of the crop in the world as a Collectable
    TODO: Make the numSpawnedCrops variable based upon a pair of string tags in the crop template indicating minimum and maximum yield
    */
    public void HarvestCrop() {
        //Only yield any items if the crop was fully grown
        if(growthStage >= maxGrowthStage) {
            int numSpawnedCrops = Random.Range(2, 6);
            Collectable spawnedItem = Collectable.Spawn(transform.position, cropTemplate.GetTagValue("grows_into"), numSpawnedCrops, 1.5f);
            spawnedItem.ApplyRandomForce(8.0f);
        }

        Destroy(this.gameObject);
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
        return activeGrid.CellToWorld(gridPosition) + new Vector3(0.5f, -0.5f, 0.0f); //Offset by half so the crop is planted in the middle
    }

    /*
    Checks if a provided position is in a plantable area, i.e. a GameObject with the PlantableArea tag that also has a PlantableArea script attached
    @return True if the provided coordinate was in any plantable area, false if not
    */
    public static bool CheckInPlantableArea(Vector3 position) {
        GameObject[] areas = GameObject.FindGameObjectsWithTag("PlantableArea");
        if(areas.Length == 0) //Return if no areas were found with the tag
            return false;

        foreach(GameObject obj in areas) {
            PlantableArea area = obj.GetComponent<PlantableArea>();
            if((area != null) & area.ContainsPoint(position)) {
                return true;
            }
        }

        return false;
    }

    /*
    Checks if a crop can be planted at the provided position
    @param The grid to check on
    @param position The position to test
    */
    public static bool CanPlant(Vector3 position) {
        Vector3 gridPosition = SnapPositionToGrid(position);
        //Check if the grid position is in a plantable area
        if(!CheckInPlantableArea(gridPosition))
            return false;

        //Check if the crop is overlapping any existing crops.
        //TODO: Make 1<<7 more readable. Currently this tells the function to only check overlaps on layer 7
        Collider2D[] results = Physics2D.OverlapCircleAll(gridPosition, 0.5f, 1<<7); 

        //Return true if there were 0 crops at this cell
        return results.Length == 0;
    }
}
