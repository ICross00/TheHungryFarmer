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

    private int minYield;
    private int maxYield;

    protected override void Start() {
        base.Start();

        gameObject.tag = "Crop";
        //Set growth probabilities
        this.eventProbability = 0.1f;
        this.period = 2.5f;

        //Locate the array of sprites associated with crop growth stages
        SpriteListDictionary cropDict = Resources.Load<SpriteListDictionary>("Prefabs/Crop Sprite Dictionary");

        //Get the key associated with the appropriate crop sprites
        string cropType = cropTemplate.GetTagValue("crop_sprites");
        cropSprites = cropDict.GetSpriteList(cropType);
        maxGrowthStage = cropSprites.Length;

        //Set initial sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cropSprites[0];

        //Find plantable grid and snap to grid
        plantableGrid = GridUtilities.GetActiveGrid();
        this.transform.position = GridUtilities.SnapPositionToGrid(this.transform.position);

        //Find plantable area
        GameObject pObj = GameObject.Find("PlantableArea");
        plantableArea = pObj.GetComponent<PlantableArea>();

        //Assign unique name
        this.name = cropType + "_" + GetInstanceID();
        this.transform.parent = pObj.transform;

        //Get minimum and maximum yield values
        minYield = int.Parse(cropTemplate.GetTagValue("min_yield"));
        maxYield = int.Parse(cropTemplate.GetTagValue("max_yield"));
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
    */
    public void HarvestCrop() {
        //Only yield any items if the crop was fully grown
        if(growthStage >= maxGrowthStage) {
            int numSpawnedCrops = Random.Range(minYield, maxYield);
            Collectable spawnedItem = Collectable.Spawn(transform.position, cropTemplate.GetTagValue("grows_into"), numSpawnedCrops, 1.5f);
            spawnedItem.ApplyRandomForce(8.0f);
            Destroy(this.gameObject);
        }
    }

    /*
    Checks if a crop can be planted at the provided position
    @param The grid to check on
    @param position The position to test
    */
    public static bool CanPlant(Vector3 position) {
        Vector3 gridPosition = GridUtilities.SnapPositionToGrid(position);
        //Check if the grid position is in a plantable area
        if(!PlantableArea.CheckInPlantableArea(gridPosition))
            return false;

        //Check if the crop is overlapping any existing crops.
        Collider2D[] results = Physics2D.OverlapCircleAll(gridPosition, 0.5f, 1<<7);

        //Return true if there were 0 crops at this cell
        return results.Length == 0;
    }
}
