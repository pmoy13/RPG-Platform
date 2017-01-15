using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicGrid : MonoBehaviour
{

    public int Width = 6;
    public int Height = 6;

    public float ScaleFactor = 10f;

    public int GetNumVertices()
    {
        return Width * Height;
    }

    public abstract int GetMaxNeighbors();

    public abstract BasicCell GetBasicCell(int index);
}
