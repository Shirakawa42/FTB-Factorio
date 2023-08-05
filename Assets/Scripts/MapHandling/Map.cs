using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map
{
    public static Dictionary<Vector2Int, Chunk> FloorChunks = new();
    public static Dictionary<Vector2Int, Chunk> SolidChunks = new();

    public static void LoadAroundChunkPosition(Vector2Int position)
    {
        for (int x = position.x - Globals.LoadDistance; x < position.x + Globals.LoadDistance; x++)
        {
            for (int y = position.y - Globals.LoadDistance; y < position.y + Globals.LoadDistance; y++)
            {
                if (!FloorChunks.ContainsKey(new Vector2Int(x, y)))
                {
                    FloorChunks.Add(new Vector2Int(x, y), new Chunk(new Vector2Int(x, y), WorldsIds.overworld, ChunkTypes.Floor));
                }
                if (!SolidChunks.ContainsKey(new Vector2Int(x, y)))
                {
                    SolidChunks.Add(new Vector2Int(x, y), new Chunk(new Vector2Int(x, y), WorldsIds.overworld, ChunkTypes.Solid));
                }
            }
        }
    }
}
