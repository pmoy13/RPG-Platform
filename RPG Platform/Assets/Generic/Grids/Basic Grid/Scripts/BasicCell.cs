using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicCell : MonoBehaviour
{
    // Dangerous terrain costs double to move over.
    public bool IsDangerousTerrain = false;

    // Some terrain cannot be traversed.
    public bool IsWalkable = true;

    // TODO: Description.
    public abstract int GetIndexFromCoordinates(int gridWidth);

    // TODO: Description.
    public abstract BasicCell[] GetNeighbors();
}
