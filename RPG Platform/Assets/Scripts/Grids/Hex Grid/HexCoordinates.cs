using UnityEngine;

// Making the class serializable allows instances to survive recompiles
// while in play mode, since Unity stores the class.
[System.Serializable]
public class HexCoordinates
{
    // Read only coordinates for a HexCell.
    public int X { get; private set; }
    public int Z { get; private set; }

    public HexCoordinates(int x, int z)
    {
        X = x;
        Z = z;
    }

    /*
     * Method:
     *   FromOffsetCoordinates
     *   
     * Description:
     *   Creates cube coordinates from offset coordinates.
     *   Needed since we create the HexCells with offset
     *   coordinates, but they're hard to work with.
     */
    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        // In the same row, adjacent hexagons are not at the same
        // height. They alternate lower and higher, so we must account
        // for that.
        int verticalRowOffset = x / 2;
        return new HexCoordinates(x, z - verticalRowOffset);
    }
}
