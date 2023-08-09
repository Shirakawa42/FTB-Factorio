using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static ushort GetFloorBlockAtWorldPosition(Vector2Int position, WorldsIds worldId)
    {
        float noise = Mathf.PerlinNoise(position.x / 10f + Globals.Seed, position.y / 10f + Globals.Seed);
        if (noise < 0.5f)
            return ItemIds.Grass;
        else
            return ItemIds.Stone;
    }

    public static ushort GetSolidBlockAtWorldPosition(Vector2Int position, WorldsIds worldId)
    {
        float noise = Mathf.PerlinNoise(position.x / 5f + Globals.Seed, position.y / 5f + Globals.Seed);
        if (noise < 0.5f)
            return ItemIds.Air;
        else
            return ItemIds.Wood;
    }
}
