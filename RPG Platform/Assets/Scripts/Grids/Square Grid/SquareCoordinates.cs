using UnityEngine;

// Serializing the class allows Unity to save it
// and the class will persist across recompilations.
[System.Serializable]
public struct SquareCoordinates
{
    // Immutable coordinates are read-only.
    [SerializeField] private int _x;
    [SerializeField] private int _z;

    // Accessor methods for the coordinats.
    public int X
    {
        get { return _x; }
    }

    public int Z
    {
        get { return _z; }
        
    }

    public SquareCoordinates(int x, int z)
    {
        _x = x;
        _z = z;
    }

    public static SquareCoordinates FromPosition(Vector3 position, float scaleFactor)
    {
        float x = position.x / scaleFactor;
        float z = position.z / scaleFactor;

        return new SquareCoordinates(Mathf.FloorToInt(x), Mathf.FloorToInt(z));
    }
}