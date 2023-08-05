using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public const int ChunkSize = 64;
    public const int LoadDistance = 2;
    public const int Seed = 56231;

    public static GameObject Player;

    public static Vector2Int GetChunkPositionFromWorldPosition(Vector2Int worldPosition)
    {
        return new Vector2Int(worldPosition.x / ChunkSize, worldPosition.y / ChunkSize);
    }
}
