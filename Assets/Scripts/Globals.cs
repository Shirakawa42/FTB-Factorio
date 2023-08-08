using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public const int ChunkSize = 64;
    public const int LoadDistance = 2;
    public const int Seed = 56231;
    public const int ToolbarSlots = 9;
    public const int InventorySlots = 27;

    public static GameObject Player;
    public static GameObject BlockBreaking;
    public static Sprites Sprites;

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

    public static PrimaryBlocks GetSolidBlockStatsFromWorldPosition(Vector3 worldPosition)
    {
        Vector2Int chunkPosition = GetChunkPositionFromWorldPosition(worldPosition);
        Vector2Int blockPosition = GetBlockPositionFromWorldPosition(worldPosition);

        if (Map.SolidChunks.ContainsKey(chunkPosition))
            return (PrimaryBlocks)ItemInfos.GetItemFromId(Map.SolidChunks[chunkPosition].Blocs[blockPosition.x + blockPosition.y * ChunkSize].Id);
        return (PrimaryBlocks)ItemInfos.GetItemFromId(ItemIds.Air);
    }

    public static void SetSolidBlockStatsAtWorldPosition(Vector3 worldPosition, ushort id)
    {
        Vector2Int chunkPosition = GetChunkPositionFromWorldPosition(worldPosition);
        Vector2Int blockPosition = GetBlockPositionFromWorldPosition(worldPosition);

        if (Map.SolidChunks.ContainsKey(chunkPosition))
            Map.SolidChunks[chunkPosition].AddBlock(blockPosition, id);
    }

    public static void RemoveBlockAtWorldPosition(Vector3 worldPosition)
    {
        Vector2Int chunkPosition = GetChunkPositionFromWorldPosition(worldPosition);
        Vector2Int blockPosition = GetBlockPositionFromWorldPosition(worldPosition);

        if (Map.SolidChunks.ContainsKey(chunkPosition))
            Map.SolidChunks[chunkPosition].AddBlock(blockPosition, ItemIds.Air);
    }

    public static Vector2Int WorldPositionToVector2Int(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y));
    }
}
