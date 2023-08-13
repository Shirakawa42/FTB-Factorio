using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

public class ChunksManager : MonoBehaviour
{
    private Vector2Int _playerChunkPosition;
    private WorldsIds _oldWorldId = WorldsIds.overworld;
    private Dictionary<MapKey, Chunk> FloorChunks = new();
    private Dictionary<MapKey, Chunk> SolidChunks = new();

    private List<Vector2Int> _chunksToLoad = new();

    void Start()
    {
        _playerChunkPosition = new Vector2Int(int.MaxValue, int.MaxValue);
    }

    private IEnumerator LoadChunks()
    {
        while (_chunksToLoad.Count > 0)
        {
            Vector2Int chunkPosition = _chunksToLoad[0];
            _chunksToLoad.RemoveAt(0);
            LoadAroundChunkPosition(chunkPosition, Globals.CurrentWorldId);
            yield return null;
        }
    }

    private void LoadAroundPlayer()
    {
        Vector3 _playerPosition = Globals.Player.transform.position;
        Vector2Int chunkPosition = new((int)_playerPosition.x / Globals.ChunkSize, (int)_playerPosition.y / Globals.ChunkSize);

        if (chunkPosition != _playerChunkPosition || Globals.CurrentWorldId != _oldWorldId)
        {
            _oldWorldId = Globals.CurrentWorldId;
            _playerChunkPosition = chunkPosition;
            LoadAroundChunkPosition(_playerChunkPosition, Globals.CurrentWorldId);
        }
    }

    void Update()
    {
        LoadAroundPlayer();
    }

    public void LoadAroundChunkPosition(Vector2Int position, WorldsIds worldId)
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

    public Chunk GetChunk(Vector2Int position, WorldsIds worldId, ChunkTypes chunkType)
    {
        if (chunkType == ChunkTypes.Floor)
        {
            if (FloorChunks.ContainsKey(new MapKey(position, worldId)))
                return FloorChunks[new MapKey(position, worldId)];
            return null;
        }
        else if (chunkType == ChunkTypes.Solid)
        {
            if (SolidChunks.ContainsKey(new MapKey(position, worldId)))
                return SolidChunks[new MapKey(position, worldId)];
            return null;
        }
        return null;
    }

}
