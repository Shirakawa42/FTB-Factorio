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
    private Stack<MapKey> _chunksToPreload = new();

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
                if (_chunksToPreload.Count > 0)
                {
                    MapKey key = _chunksToPreload.Pop();
                    MapValue chunks = _chunks[key];
                    chunks.FloorChunk.PreloadChunk();
                    chunks.SolidChunk.PreloadChunk();
                }
            }
            if (_chunksToPreload.Count == 0)
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
            }
            yield return null;
        }
    }

    private void LoadAroundPlayer()
    {
        Vector3 _playerPosition = Globals.Player.transform.position;
        Vector2Int chunkPosition = WorldsHelper.GetChunkPositionFromWorldPosition(_playerPosition);

        if (chunkPosition != _playerChunkPosition || Globals.CurrentWorldId != _oldWorldId)
        {
            _oldWorldId = Globals.CurrentWorldId;
            _playerChunkPosition = chunkPosition;
            PreloadAroundChunkPosition(_playerChunkPosition, Globals.CurrentWorldId);
            LoadAroundChunkPosition(_playerChunkPosition, Globals.CurrentWorldId);
        }
    }

    void Update()
    {
        LoadAroundPlayer();
    }

    private void LoadAroundChunkPosition(Vector2Int position, WorldsIds worldId)
    {
        for (int x = position.x - Globals.LoadDistance; x <= position.x + Globals.LoadDistance; x++)
        {
            for (int y = position.y - Globals.LoadDistance; y <= position.y + Globals.LoadDistance; y++)
            {
                MapKey key = new(new Vector2Int(x, y), worldId);
                if (!_chunks.ContainsKey(key))
                    _chunks.Add(key, new MapValue(new Chunk(new Vector2Int(x, y), worldId, ChunkTypes.Floor), new Chunk(new Vector2Int(x, y), worldId, ChunkTypes.Solid)));
                _chunksToLoad.Push(key);
            }
        }
    }

    private void PreloadAroundChunkPosition(Vector2Int position, WorldsIds worldId)
    {
        for (int x = position.x - Globals.PreloadDistance; x <= position.x + Globals.PreloadDistance; x++)
        {
            for (int y = position.y - Globals.PreloadDistance; y <= position.y + Globals.PreloadDistance; y++)
            {
                MapKey key = new(new Vector2Int(x, y), worldId);
                if (!_chunks.ContainsKey(key))
                    _chunks.Add(key, new MapValue(new Chunk(new Vector2Int(x, y), worldId, ChunkTypes.Floor), new Chunk(new Vector2Int(x, y), worldId, ChunkTypes.Solid)));
                _chunksToPreload.Push(key);
            }
        }
    }

    private const int MAX_CACHED_CHUNKS = 4;
    private static readonly Chunk[] cachedSolidChunks = new Chunk[MAX_CACHED_CHUNKS];
    private static readonly Chunk[] cachedFloorChunks = new Chunk[MAX_CACHED_CHUNKS];
    private static int currentSolidIndex = 0;
    private static int currentFloorIndex = 0;

    public Chunk GetChunk(Vector2Int position, WorldsIds worldId, ChunkTypes chunkType)
    {
        Chunk[] currentCache;
        int currentIndex;
        if (chunkType == ChunkTypes.Solid)
        {
            currentCache = cachedSolidChunks;
            currentIndex = currentSolidIndex;
        }
        else if (chunkType == ChunkTypes.Floor)
        {
            currentCache = cachedFloorChunks;
            currentIndex = currentFloorIndex;
        }
        else
            throw new System.Exception("Chunk type not found");

        for (int i = 0; i < MAX_CACHED_CHUNKS; i++)
        {
            Chunk chunk = currentCache[i];
            if (chunk != null && chunk.Position == position && chunk.WorldId == worldId && chunk.ChunkType == chunkType)
                return chunk;
        }

        MapKey key = new MapKey(position, worldId);
        if (!_chunks.ContainsKey(key))
            _chunks.Add(key, new MapValue(new Chunk(position, worldId, ChunkTypes.Floor), new Chunk(position, worldId, ChunkTypes.Solid)));

        Chunk desiredChunk;
        if (chunkType == ChunkTypes.Floor)
            desiredChunk = _chunks[key].FloorChunk;
        else
            desiredChunk = _chunks[key].SolidChunk;

        currentCache[currentIndex] = desiredChunk;
        if (chunkType == ChunkTypes.Solid)
            currentSolidIndex = (currentSolidIndex + 1) % MAX_CACHED_CHUNKS;
        else
            currentFloorIndex = (currentFloorIndex + 1) % MAX_CACHED_CHUNKS;

        return desiredChunk;
    }


    private HashSet<Vector2Int> _lightSourcesToRecalculate = new();
    public HashSet<MapKey> modifiedChunks = new();

    private void FloodFill(Vector2Int blockWorldPosition, byte lightIntensity, bool removeLight, WorldsIds worldsId, bool isSource)
    {
        if (lightIntensity == 0)
            return;

        if (removeLight)
            SetLight(blockWorldPosition, ItemInfos.GetPrimaryBlockFromId(GetBlock(blockWorldPosition, worldsId, ChunkTypes.Solid).Id).LightSourcePower, worldsId);
        else
            SetLight(blockWorldPosition, lightIntensity, worldsId);

        Chunk chunk = GetChunk(WorldsHelper.GetChunkPositionFromWorldPosition(new Vector3(blockWorldPosition.x, blockWorldPosition.y, 0)), worldsId, ChunkTypes.Solid);
        if (!modifiedChunks.Contains(new MapKey(chunk.Position, worldsId)))
            modifiedChunks.Add(new MapKey(chunk.Position, worldsId));

        byte newlightIntensity;
        if (isSource || ItemInfos.GetPrimaryBlockFromId(GetBlock(blockWorldPosition, worldsId, ChunkTypes.Solid).Id).IsTransparent)
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
            FloodFill(new Vector2Int(blockWorldPosition.x - 1, blockWorldPosition.y), newlightIntensity, removeLight, worldsId, false);

        if (rightBlock.Light >= lightIntensity && removeLight)
            _lightSourcesToRecalculate.Add(new Vector2Int(blockWorldPosition.x + 1, blockWorldPosition.y));
        else if (rightBlock.Light <= newlightIntensity)
            FloodFill(new Vector2Int(blockWorldPosition.x + 1, blockWorldPosition.y), newlightIntensity, removeLight, worldsId, false);

        if (upBlock.Light >= lightIntensity && removeLight)
            _lightSourcesToRecalculate.Add(new Vector2Int(blockWorldPosition.x, blockWorldPosition.y + 1));
        else if (upBlock.Light <= newlightIntensity)
            FloodFill(new Vector2Int(blockWorldPosition.x, blockWorldPosition.y + 1), newlightIntensity, removeLight, worldsId, false);

        if (downBlock.Light >= lightIntensity && removeLight)
            _lightSourcesToRecalculate.Add(new Vector2Int(blockWorldPosition.x, blockWorldPosition.y - 1));
        else if (downBlock.Light <= newlightIntensity)
            FloodFill(new Vector2Int(blockWorldPosition.x, blockWorldPosition.y - 1), newlightIntensity, removeLight, worldsId, false);
    }

    public void StartFloodFill(Vector2Int blockWorldPosition, byte lightIntensity, bool removeLight, WorldsIds worldId)
    {
        FloodFill(blockWorldPosition, lightIntensity, removeLight, worldId, true);
        while (removeLight && _lightSourcesToRecalculate.Count > 0)
        {
            blockWorldPosition = _lightSourcesToRecalculate.First();
            _lightSourcesToRecalculate.Remove(blockWorldPosition);
            lightIntensity = GetBlock(blockWorldPosition, worldId, ChunkTypes.Solid).Light;
            FloodFill(blockWorldPosition, lightIntensity, false, worldId, false);
        }
    }

    public void ReloadModifiedChunks()
    {
        foreach (MapKey key in modifiedChunks)
            _chunksToLoad.Push(key);
        modifiedChunks.Clear();
    }

    public Block GetBlock(Vector2Int blockWorldPosition, WorldsIds worldId, ChunkTypes chunkType)
    {
        Vector2Int chunkPosition = WorldsHelper.GetChunkPositionFromWorldPosition(new Vector3(blockWorldPosition.x, blockWorldPosition.y, 0));
        ushort blockPosition = WorldsHelper.GetBlockPositionFromWorldPosition(new Vector3(blockWorldPosition.x, blockWorldPosition.y, 0));

        Chunk chunk = GetChunk(chunkPosition, worldId, chunkType);
        if (chunk != null && chunk.IsLoaded())
            return chunk.GetBlock(blockPosition);
        return new Block(ItemIds.Air, 0);
    }

    public void SetLight(Vector2Int blockWorldPosition, byte lightLevel, WorldsIds worldId)
    {
        Vector2Int chunkPosition = WorldsHelper.GetChunkPositionFromWorldPosition(new Vector3(blockWorldPosition.x, blockWorldPosition.y, 0));
        ushort blockPosition = WorldsHelper.GetBlockPositionFromWorldPosition(new Vector3(blockWorldPosition.x, blockWorldPosition.y, 0));

        Chunk chunk = GetChunk(chunkPosition, worldId, ChunkTypes.Solid);
        if (chunk != null && chunk.IsLoaded())
            chunk.SetLight(blockPosition, lightLevel);
        chunk = GetChunk(chunkPosition, worldId, ChunkTypes.Floor);
        if (chunk != null && chunk.IsLoaded())
            chunk.SetLight(blockPosition, lightLevel);
    }

}
