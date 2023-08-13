using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapKey
{
    public Vector2Int Position;
    public WorldsIds WorldId;
    public ChunkTypes ChunkType;

    public MapKey(Vector2Int position, WorldsIds worldId, ChunkTypes chunkType)
    {
        Position = position;
        WorldId = worldId;
        ChunkType = chunkType;
    }
}

public class ChunksManager : MonoBehaviour
{
    private Vector2Int _playerChunkPosition;
    private WorldsIds _oldWorldId = WorldsIds.overworld;
    private Dictionary<MapKey, Chunk> _chunks = new();
    private readonly Stack<MapKey> _chunksToLoad = new();

    void Start()
    {
        _playerChunkPosition = new Vector2Int(int.MaxValue, int.MaxValue);
        StartCoroutine(LoadChunks());
    }

    private IEnumerator LoadChunks()
    {
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                if (_chunksToLoad.Count > 0)
                {
                    MapKey key = _chunksToLoad.Pop();
                    Chunk chunk = _chunks[key];
                    chunk.ReloadChunk();
                }
            }
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
                MapKey key = new(new Vector2Int(x, y), worldId, ChunkTypes.Floor);
                if (!_chunks.ContainsKey(key))
                    _chunks.Add(key, new Chunk(new Vector2Int(x, y), worldId, ChunkTypes.Floor));
                _chunksToLoad.Push(key);
                key.ChunkType = ChunkTypes.Solid;
                if (!_chunks.ContainsKey(key))
                    _chunks.Add(key, new Chunk(new Vector2Int(x, y), worldId, ChunkTypes.Solid));
                _chunksToLoad.Push(key);
            }
        }
    }

    public Chunk GetChunk(Vector2Int position, WorldsIds worldId, ChunkTypes chunkType)
    {
        MapKey key = new(position, worldId, chunkType);
        if (!_chunks.ContainsKey(key))
            _chunks.Add(key, new Chunk(position, worldId, chunkType));
        return _chunks[key];
    }

}
