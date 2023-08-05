using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static ushort GetBlockAtWorldPosition(Vector2Int position)
    {
        float noise = Mathf.PerlinNoise(position.x / 10f + Globals.Seed, position.y / 10f + Globals.Seed);
        if (noise < 0.5f)
            return BlockIds.Grass;
        else
            return BlockIds.Stone;
    }
}
