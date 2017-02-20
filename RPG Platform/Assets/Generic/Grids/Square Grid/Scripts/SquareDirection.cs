using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SquareDirection
{
    N,
    NE,
    E,
    SE,
    S,
    SW,
    W,
    NW,
    Invalid
}

public static class SquareDirectionExtensions
{
    public static bool IsDiagonal(this SquareDirection direction)
    {
        if ((int) direction % 2 == 0)
        {
            return false;
        }

        return true;
    }
}
