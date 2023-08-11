using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static ushort GetFloorBlockAtWorldPosition(Vector2Int position, WorldsIds worldId)
    {
        if (worldId == WorldsIds.overworld)
            return OverWorldFloorGenerator(position);
        else if (worldId == WorldsIds.low_depth)
            return LowDepthFloorGenerator(position);
        else
            return ItemIds.Air;
    }

    public static ushort GetSolidBlockAtWorldPosition(Vector2Int position, WorldsIds worldId)
    {
        if (worldId == WorldsIds.overworld)
            return OverWorldSolidGenerator(position);
        else if (worldId == WorldsIds.low_depth)
            return LowDepthSolidGenerator(position);
        else
            return ItemIds.Air;
    }

    private static ushort LowDepthFloorGenerator(Vector2Int position)
    {
        return ItemIds.Stone;
    }

    private static ushort LowDepthSolidGenerator(Vector2Int position)
    {
        float noise = Mathf.PerlinNoise(position.x / 5f + Globals.Seed, position.y / 5f + Globals.Seed);
        if (noise < 0.5f)
            return ItemIds.Stone;
        else
            return ItemIds.Air;
    }

    private static ushort OverWorldFloorGenerator(Vector2Int position)
    {
        float noise = Mathf.PerlinNoise(position.x / 10f + Globals.Seed, position.y / 10f + Globals.Seed);
        if (noise < 0.5f)
            return ItemIds.Grass;
        else
            return ItemIds.Stone;
    }

    private static ushort OverWorldSolidGenerator(Vector2Int position)
    {
        float noise = Mathf.PerlinNoise(position.x / 5f + Globals.Seed, position.y / 5f + Globals.Seed);
        if (noise < 0.5f)
            return ForestGenerator(position);
        else
            return ItemIds.Stone;
    }

    private static ushort ForestGenerator(Vector2Int position)
    {
        int treeSpacing = 3;
        Vector2Int samplePosition = new(Mathf.FloorToInt(position.x / treeSpacing) * treeSpacing,
                                                   Mathf.FloorToInt(position.y / treeSpacing) * treeSpacing);

        if (Mathf.Abs(position.x - samplePosition.x) <= 1 && Mathf.Abs(position.y - samplePosition.y) <= 1)
        {
            float treeNoise = Mathf.PerlinNoise((position.x / 2f) + (Globals.Seed * 3), (position.y / 2f) + (Globals.Seed * 3));

            if (treeNoise > 0.8f)
                return ItemIds.Wood;
        }

        return ItemIds.Air;
    }
}
