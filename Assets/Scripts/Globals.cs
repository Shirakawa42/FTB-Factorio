using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public const int ChunkSize = 64;
    public const int LoadDistance = 2;
    public const int Seed = 56231;

    public static GameObject Player;

    public static Vector2Int GetChunkPositionFromWorldPosition(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPosition.x / ChunkSize), Mathf.FloorToInt(worldPosition.y / ChunkSize));
    }

    public static Vector2Int GetBlockPositionFromWorldPosition(Vector3 worldPosition)
    {
        int blockPosX = Mathf.FloorToInt(worldPosition.x);
        int blockPosY = Mathf.FloorToInt(worldPosition.y);

        blockPosX = (blockPosX % ChunkSize + ChunkSize) % ChunkSize;
        blockPosY = (blockPosY % ChunkSize + ChunkSize) % ChunkSize;

        return new Vector2Int(blockPosX, blockPosY);
    }

    public static BlockStats GetSolidBlockStatsFromWorldPosition(Vector3 worldPosition)
    {
        Vector2Int chunkPosition = GetChunkPositionFromWorldPosition(worldPosition);
        Vector2Int blockPosition = GetBlockPositionFromWorldPosition(worldPosition);

        if (Map.SolidChunks.ContainsKey(chunkPosition))
            return AllBlocksStats.GetBlockStatsFromId(Map.SolidChunks[chunkPosition].Blocs[blockPosition.x + blockPosition.y * ChunkSize].Id);
        return AllBlocksStats.GetBlockStatsFromId(BlockIds.Air);
    }
}
