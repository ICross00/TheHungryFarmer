using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Class containing utility functions for aligning objects with the scene grid
*/
public static class GridUtilities
{
    /*
    Returns the currently active Grid object
    */
    public static Grid GetActiveGrid() {
        return GameObject.Find("Grid").GetComponent<Grid>();
    }

    /*
    Snaps a vector 3 to the active grid
    @param worldPosition The position to snap to the grid
    @return The position snapped to the center of the closest grid cell
    */
    public static Vector3 SnapPositionToGrid(Vector3 worldPosition) {
        Grid activeGrid = GetActiveGrid();
        Vector3Int gridPosition = activeGrid.WorldToCell(worldPosition);
        return activeGrid.CellToWorld(gridPosition) + new Vector3(0.5f, -0.5f, 0.0f); //Offset by half so the position is in the center of a grid tile
    }

}
