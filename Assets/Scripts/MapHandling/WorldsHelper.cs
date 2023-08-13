using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldsIds
{
    overworld = 0,
    low_depth = 1,
    medium_depth = 2,
    deep_depth = 3,
    hardcore_depth = 4
}

public static class WorldsHelper
{
    public static GameObject GetWorldFromId(WorldsIds id)
    {
        if (id == WorldsIds.overworld)
            return GameObject.Find("overworld");
        else if (id == WorldsIds.low_depth)
            return GameObject.Find("low_depth");
        else if (id == WorldsIds.medium_depth)
            return GameObject.Find("medium_depth");
        else if (id == WorldsIds.deep_depth)
            return GameObject.Find("deep_depth");
        else if (id == WorldsIds.hardcore_depth)
            return GameObject.Find("hardcore_depth");
        else
            throw new System.Exception($"WorldsIds {id} not found");
    }

    public static Vector2Int GetChunkPositionFromWorldPosition(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPosition.x / Globals.ChunkSize), Mathf.FloorToInt(worldPosition.y / Globals.ChunkSize));
    }

    public static Vector2Int GetBlockPositionFromWorldPosition(Vector3 worldPosition)
    {
        int blockPosX = Mathf.FloorToInt(worldPosition.x);
        int blockPosY = Mathf.FloorToInt(worldPosition.y);

        blockPosX = (blockPosX % Globals.ChunkSize + Globals.ChunkSize) % Globals.ChunkSize;
        blockPosY = (blockPosY % Globals.ChunkSize + Globals.ChunkSize) % Globals.ChunkSize;

        return new Vector2Int(blockPosX, blockPosY);
    }

    public static PrimaryBlocks GetBlockStats(Vector3 worldPosition, WorldsIds worldId, ChunkTypes chunkType)
    {
        Vector2Int chunkPosition = GetChunkPositionFromWorldPosition(worldPosition);
        Vector2Int blockPosition = GetBlockPositionFromWorldPosition(worldPosition);

        Chunk chunk = Globals.ChunksManager.GetChunk(chunkPosition, worldId, chunkType);
        if (chunk != null)
            return (PrimaryBlocks)ItemInfos.GetItemFromId(chunk.GetBlockId(blockPosition));
        return (PrimaryBlocks)ItemInfos.GetItemFromId(ItemIds.Air);
    }

    public static void SetBlock(Vector3 worldPosition, ushort id, WorldsIds worldId, ChunkTypes chunkType)
    {
        Vector2Int chunkPosition = GetChunkPositionFromWorldPosition(worldPosition);
        Vector2Int blockPosition = GetBlockPositionFromWorldPosition(worldPosition);

        Chunk chunk = Globals.ChunksManager.GetChunk(chunkPosition, worldId, chunkType);
        if (chunk != null)
            chunk.AddBlock(blockPosition, id);
    }

    public static Vector2Int WorldPositionToVector2Int(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y));
    }
}
