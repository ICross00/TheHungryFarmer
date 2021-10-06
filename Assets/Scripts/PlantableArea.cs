using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantableArea : MonoBehaviour
{
    public Vector2 boundSize;
    private Bounds plantableBoundary;

    // Set the boundaries from the top-left
    void Start()
    {
        gameObject.tag = "PlantableArea";
        plantableBoundary = new Bounds(transform.position, boundSize);
    }

    /**
    Checks if the Bounds object contains a given point
    @param point The point that should be checked for inside the bounds
    @return True if the point was inside the bounds, false if not
    */
    public bool ContainsPoint(Vector3 point) {
        return plantableBoundary.Contains(point);
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

    //Draw debug outlines
    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, boundSize);
    }

}
