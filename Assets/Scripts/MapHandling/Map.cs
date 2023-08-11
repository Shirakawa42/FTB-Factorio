using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapKey
{
    public Vector2Int Position;
    public WorldsIds WorldId;

    public MapKey(Vector2Int position, WorldsIds worldId)
    {
        Position = position;
        WorldId = worldId;
    }
}

public static class Map
{
    public static Dictionary<MapKey, Chunk> FloorChunks = new();
    public static Dictionary<MapKey, Chunk> SolidChunks = new();

    public static void LoadAroundChunkPosition(Vector2Int position, WorldsIds worldId)
    {
        for (int x = position.x - Globals.LoadDistance; x < position.x + Globals.LoadDistance; x++)
        {
            for (int y = position.y - Globals.LoadDistance; y < position.y + Globals.LoadDistance; y++)
            {
                MapKey key = new(new Vector2Int(x, y), worldId);
                if (!FloorChunks.ContainsKey(key))
                {
                    FloorChunks.Add(key, new Chunk(new Vector2Int(x, y), worldId, ChunkTypes.Floor));
                }
                if (!SolidChunks.ContainsKey(key))
                {
                    SolidChunks.Add(key, new Chunk(new Vector2Int(x, y), worldId, ChunkTypes.Solid));
                }
            }
        }
    }
}
