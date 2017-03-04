/*
 * File:
 *   HexMetrics.cs
 * 
 * Description:
 *   This file contains a static class and static methods
 *   used to create hexagonal grids of variable size.
 */

using UnityEngine;

/*
 * Class:
 *   HexMetrics
 *   
 * Description:
 *   This is a static class containing definitions for
 *   creating and manipulating hexagonal grids.
 */
public static class HexMetrics
{

	// Distance from the center of a unit-sized hexagon
    // to any vertex of the hexagon.
    public const float OuterUnitRadius = 1f;

    // Distance from the center of a unit-sized hexagon
    // to any edge of the hexagon. In a hexagonal grid,
    // the distance between two adjacent hexagons is
    // twice this value.
    public const float InnerUnitRadius = OuterUnitRadius * 0.866025404f;

    // Definitions of the six corners of a hexagon relative
    // to its center. We have chosen to orient the hexagon with flat
    // edges on the top and bottom.
    // Vertex numbering is as follows:
    //     4 * 5
    //    *     *
    //   3       0 = 6
    //    *     *
    //     2 * 1
    // We need vertex 0 twice (again as vertex 6) because when we construct
    // a hexagon from triangles we'd get an out of bounds error without it.
    public static Vector3[] CornerVector3S =
    {
        new Vector3(OuterUnitRadius, 0f, 0f), // 0
        new Vector3(0.5f * OuterUnitRadius, 0f, -InnerUnitRadius), // 1
        new Vector3(-0.5f * OuterUnitRadius, 0f, -InnerUnitRadius), // 2
        new Vector3(-OuterUnitRadius, 0f, 0f), // 3
        new Vector3(-0.5f * OuterUnitRadius, 0f, InnerUnitRadius), // 4
        new Vector3(0.5f * OuterUnitRadius, 0f, InnerUnitRadius), // 5
        new Vector3(OuterUnitRadius, 0f, 0f) // 6
    };

    // Used when creating a hexagon from 6 equilateral triangles.
    public const int TrianglesPerHexagon = 6;

    // Each hexagon has 6 neighbors.
    public const int NeighborsPerHexagon = 6;
}
