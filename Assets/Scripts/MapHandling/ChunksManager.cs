using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

public struct MapValue
{
    public Chunk FloorChunk;
    public Chunk SolidChunk;

    public MapValue(Chunk floorChunk, Chunk solidChunk)
    {
        FloorChunk = floorChunk;
        SolidChunk = solidChunk;
    }
}

public class ChunksManager : MonoBehaviour
{
    private Vector2Int _playerChunkPosition;
    private WorldsIds _oldWorldId = WorldsIds.overworld;
    private Dictionary<MapKey, MapValue> _chunks = new();
    private Stack<MapKey> _chunksToLoad = new();

    void Start()
    {
        _playerChunkPosition = new Vector2Int(int.MaxValue, int.MaxValue);
        StartCoroutine(LoadChunks());
    }

    private IEnumerator LoadChunks()
    {
        while (true)
        {
            for (int i = 0; i < 2; i++)
            {
                if (_chunksToLoad.Count > 0)
                {
                    MapKey key = _chunksToLoad.Pop();
                    MapValue chunks = _chunks[key];
                    chunks.FloorChunk.ReloadChunk();
                    chunks.SolidChunk.ReloadChunk();
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
                MapKey key = new(new Vector2Int(x, y), worldId);
                if (!_chunks.ContainsKey(key))
                    _chunks.Add(key, new MapValue(new Chunk(new Vector2Int(x, y), worldId, ChunkTypes.Floor), new Chunk(new Vector2Int(x, y), worldId, ChunkTypes.Solid)));
                _chunksToLoad.Push(key);
            }
        }
    }

    public Chunk GetChunk(Vector2Int position, WorldsIds worldId, ChunkTypes chunkType)
    {
        MapKey key = new(position, worldId);
        if (!_chunks.ContainsKey(key))
            _chunks.Add(key, new MapValue(new Chunk(position, worldId, ChunkTypes.Floor), new Chunk(position, worldId, ChunkTypes.Solid)));
        if (chunkType == ChunkTypes.Floor)
            return _chunks[key].FloorChunk;
        else if (chunkType == ChunkTypes.Solid)
            return _chunks[key].SolidChunk;
        throw new System.Exception("Chunk type not found");
    }

    private static HashSet<Vector2Int> _lightSourcesToRecalculate = new();
    private HashSet<MapKey> _modifiedChunks = new();

    private void FloodFill(Vector2Int blockWorldPosition, byte lightIntensity, bool removeLight, WorldsIds worldsId)
    {
        if (lightIntensity == 0)
            return;

        if (removeLight)
            SetLight(blockWorldPosition, 0, worldsId);
        else
            SetLight(blockWorldPosition, lightIntensity, worldsId);

        Chunk chunk = GetChunk(WorldsHelper.GetChunkPositionFromWorldPosition(new Vector3(blockWorldPosition.x, blockWorldPosition.y, 0)), worldsId, ChunkTypes.Solid);
        if (!_modifiedChunks.Contains(new MapKey(chunk.Position, worldsId)))
            _modifiedChunks.Add(new MapKey(chunk.Position, worldsId));

        byte newlightIntensity;
        if (removeLight || ItemInfos.GetPrimaryBlockFromId(GetBlock(blockWorldPosition, worldsId, ChunkTypes.Solid).Id).IsTransparent)
            newlightIntensity = (lightIntensity < 32) ? (byte)0 : (byte)(lightIntensity - 32);
        else
            newlightIntensity = (lightIntensity < 96) ? (byte)0 : (byte)(lightIntensity - 96);

        Block leftBlock = GetBlock(new Vector2Int(blockWorldPosition.x - 1, blockWorldPosition.y), worldsId, ChunkTypes.Solid);
        Block rightBlock = GetBlock(new Vector2Int(blockWorldPosition.x + 1, blockWorldPosition.y), worldsId, ChunkTypes.Solid);
        Block upBlock = GetBlock(new Vector2Int(blockWorldPosition.x, blockWorldPosition.y + 1), worldsId, ChunkTypes.Solid);
        Block downBlock = GetBlock(new Vector2Int(blockWorldPosition.x, blockWorldPosition.y - 1), worldsId, ChunkTypes.Solid);

        if (leftBlock.Light >= lightIntensity && removeLight)
            _lightSourcesToRecalculate.Add(new Vector2Int(blockWorldPosition.x - 1, blockWorldPosition.y));
        else if (leftBlock.Light <= newlightIntensity)
            FloodFill(new Vector2Int(blockWorldPosition.x - 1, blockWorldPosition.y), newlightIntensity, removeLight, worldsId);

        if (rightBlock.Light >= lightIntensity && removeLight)
            _lightSourcesToRecalculate.Add(new Vector2Int(blockWorldPosition.x + 1, blockWorldPosition.y));
        else if (rightBlock.Light <= newlightIntensity)
            FloodFill(new Vector2Int(blockWorldPosition.x + 1, blockWorldPosition.y), newlightIntensity, removeLight, worldsId);

        if (upBlock.Light >= lightIntensity && removeLight)
            _lightSourcesToRecalculate.Add(new Vector2Int(blockWorldPosition.x, blockWorldPosition.y + 1));
        else if (upBlock.Light <= newlightIntensity)
            FloodFill(new Vector2Int(blockWorldPosition.x, blockWorldPosition.y + 1), newlightIntensity, removeLight, worldsId);

        if (downBlock.Light >= lightIntensity && removeLight)
            _lightSourcesToRecalculate.Add(new Vector2Int(blockWorldPosition.x, blockWorldPosition.y - 1));
        else if (downBlock.Light <= newlightIntensity)
            FloodFill(new Vector2Int(blockWorldPosition.x, blockWorldPosition.y - 1), newlightIntensity, removeLight, worldsId);
    }

    public void StartFloodFill(Vector2Int blockWorldPosition, byte lightIntensity, bool removeLight, WorldsIds worldId)
    {
        FloodFill(blockWorldPosition, lightIntensity, removeLight, worldId);
        while (removeLight && _lightSourcesToRecalculate.Count > 0)
        {
            blockWorldPosition = _lightSourcesToRecalculate.First();
            _lightSourcesToRecalculate.Remove(blockWorldPosition);
            lightIntensity = GetBlock(blockWorldPosition, worldId, ChunkTypes.Solid).Light;
            FloodFill(blockWorldPosition, lightIntensity, false, worldId);
        }
        foreach (MapKey key in _modifiedChunks)
            _chunksToLoad.Push(key);
        _modifiedChunks.Clear();
    }

    public Block GetBlock(Vector2Int blockWorldPosition, WorldsIds worldId, ChunkTypes chunkType)
    {
        Vector2Int chunkPosition = WorldsHelper.GetChunkPositionFromWorldPosition(new Vector3(blockWorldPosition.x, blockWorldPosition.y, 0));
        ushort blockPosition = WorldsHelper.GetBlockPositionFromWorldPosition(new Vector3(blockWorldPosition.x, blockWorldPosition.y, 0));

        Chunk chunk = Globals.ChunksManager.GetChunk(chunkPosition, worldId, chunkType);
        if (chunk != null && chunk.IsLoaded())
            return chunk.GetBlock(blockPosition);
        return new Block(ItemIds.Air, 0);
    }

    public void SetLight(Vector2Int blockWorldPosition, byte lightLevel, WorldsIds worldId)
    {
        Vector2Int chunkPosition = WorldsHelper.GetChunkPositionFromWorldPosition(new Vector3(blockWorldPosition.x, blockWorldPosition.y, 0));
        ushort blockPosition = WorldsHelper.GetBlockPositionFromWorldPosition(new Vector3(blockWorldPosition.x, blockWorldPosition.y, 0));

        Chunk chunk = Globals.ChunksManager.GetChunk(chunkPosition, worldId, ChunkTypes.Solid);
        if (chunk != null && chunk.IsLoaded())
            chunk.SetLight(blockPosition, lightLevel);
        chunk = Globals.ChunksManager.GetChunk(chunkPosition, worldId, ChunkTypes.Floor);
        if (chunk != null && chunk.IsLoaded())
            chunk.SetLight(blockPosition, lightLevel);
    }

}
