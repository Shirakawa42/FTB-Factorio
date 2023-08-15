using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public struct Block
{
    public ushort Id;
    public byte Light;
    public byte Outlines;

    public Block(ushort id, byte light)
    {
        Id = id;
        Light = light;
        Outlines = 0;
    }
}

public enum ChunkTypes
{
    Floor,
    Solid
}

public struct ChunkModification
{
    public ushort BlockPosition;
    public ushort Id;

    public ChunkModification(ushort blockPosition, ushort id)
    {
        BlockPosition = blockPosition;
        Id = id;
    }
}

public struct LightModification
{
    public ushort BlockPosition;
    public byte Light;

    public LightModification(ushort blockPosition, byte light)
    {
        BlockPosition = blockPosition;
        Light = light;
    }
}

public class Chunk
{
    public Vector2Int Position;
    public WorldsIds WorldId;
    public ChunkTypes ChunkType;

    private readonly List<Vector3> _vertices = new();
    private readonly List<int> _indices = new();
    private readonly List<Vector2> _uvs = new();
    private readonly List<Vector2> _uvts = new();
    private readonly List<Vector2> _outlineNS = new();
    private readonly List<Vector2> _outlineEW = new();
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private GameObject _chunkGameObject;
    private Mesh _mesh;
    private readonly Dictionary<int, GameObject> _sprites = new();
    private readonly Block[] _blocs = new Block[Globals.ChunkSize * Globals.ChunkSize];
    private readonly Stack<ChunkModification> _modifications = new();
    private bool _isLoaded = false;


    public Chunk(Vector2Int position, WorldsIds worldId, ChunkTypes chunkType)
    {
        Position = position;
        WorldId = worldId;
        ChunkType = chunkType;
    }

    public void AddModification(Vector2Int blockPosition, ushort id)
    {
        _modifications.Push(new ChunkModification(GetBlockIndex(blockPosition), id));
    }

    private void ApplyModifications()
    {

    }

    private ushort GetBlockIndex(Vector2Int position)
    {
        return (ushort)(position.x + position.y * Globals.ChunkSize);
    }

    private Vector2Int GetBlockPosition(ushort index)
    {
        return new Vector2Int(index % Globals.ChunkSize, index / Globals.ChunkSize);
    }

    public void PreloadChunk()
    {
        if (!_isLoaded)
        {
            InitGameObject();
            GenerateBlocks();
            _isLoaded = true;
        }
    }

    public void ReloadChunk()
    {
        PreloadChunk();

        CalculateOutlines();
        ApplyModifications();
        GenerateVertices();
        GenerateMesh();
    }

    public bool IsLoaded()
    {
        return _isLoaded;
    }

    public ushort GetBlockId(ushort position)
    {
        return _blocs[position].Id;
    }

    public Block GetBlock(ushort position)
    {
        return _blocs[position];
    }

    public void SetLight(ushort position, byte light)
    {
        _blocs[position].Light = light;
    }

    private Block GetWorldBlock(int x, int y)
    {
        int index = x + y * Globals.ChunkSize;
        if (x >= 0 && x < Globals.ChunkSize && y >= 0 && y < Globals.ChunkSize)
            return _blocs[index];

        Vector2Int globalPos = new(Position.x * Globals.ChunkSize + x, Position.y * Globals.ChunkSize + y);
        return Globals.ChunksManager.GetBlock(globalPos, WorldId, ChunkType);
    }

    private void CalculateOutlines()
    {
        for (int x = 0; x < Globals.ChunkSize; x++)
        {
            for (int y = 0; y < Globals.ChunkSize; y++)
            {
                int i = x + y * Globals.ChunkSize;

                if (ItemInfos.GetPrimaryBlockFromId(_blocs[i].Id).IsSolid == false)
                    continue;

                byte outlines = 0;

                if (ItemInfos.GetPrimaryBlockFromId(GetWorldBlock(x - 1, y).Id).IsSolid == false)
                    outlines |= 0x1;

                if (ItemInfos.GetPrimaryBlockFromId(GetWorldBlock(x + 1, y).Id).IsSolid == false)
                    outlines |= 0x2;

                if (ItemInfos.GetPrimaryBlockFromId(GetWorldBlock(x, y - 1).Id).IsSolid == false)
                    outlines |= 0x4;

                if (ItemInfos.GetPrimaryBlockFromId(GetWorldBlock(x, y + 1).Id).IsSolid == false)
                    outlines |= 0x8;

                _blocs[i].Outlines = outlines;
            }
        }
    }

    private void CheckAndRemoveSprite(int index)
    {
        if (_sprites.ContainsKey(index))
        {
            Globals.SpritePool.ReturnSprite(_sprites[index]);
            _sprites.Remove(index);
        }
    }

    public void SetBlock(ushort index, ushort blockId)
    {
        CheckAndRemoveSprite(index);

        _blocs[index].Id = blockId;

        CheckAndAddSprite(index);

        if (_blocs[index].Light != 0)
            Globals.ChunksManager.StartFloodFill(WorldsHelper.GetBlockWorldPositionFromIndexInChunk(Position, index), _blocs[index].Light, true, WorldId);

        if (ItemInfos.GetPrimaryBlockFromId(blockId).LightSourcePower != 0)
            Globals.ChunksManager.StartFloodFill(WorldsHelper.GetBlockWorldPositionFromIndexInChunk(Position, index), ItemInfos.GetPrimaryBlockFromId(blockId).LightSourcePower, false, WorldId);

        if (_blocs[index].Light == 0 && ItemInfos.GetPrimaryBlockFromId(blockId).LightSourcePower == 0)
            ReloadChunk();
    }

    public byte GetLightIntensity(ushort position)
    {
        return _blocs[position].Light;
    }

    private Transform GetParent()
    {
        GameObject world = WorldsHelper.GetWorldFromId(WorldId);
        if (ChunkType == ChunkTypes.Floor)
            return world.transform.Find("floor");
        else if (ChunkType == ChunkTypes.Solid)
            return world.transform.Find("solid");
        throw new Exception("Chunk type does not exist or child not found");
    }

    private void InitGameObject()
    {
        _chunkGameObject = new GameObject($"Chunk {Position.x} {Position.y}");
        _chunkGameObject.transform.parent = GetParent();
        _chunkGameObject.transform.localPosition = new Vector3(Position.x * Globals.ChunkSize, Position.y * Globals.ChunkSize, 0);

        _meshFilter = _chunkGameObject.AddComponent<MeshFilter>();
        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;
        _meshRenderer = _chunkGameObject.AddComponent<MeshRenderer>();
        if (ChunkType == ChunkTypes.Floor)
            _meshRenderer.material = Globals.ChunkMaterialFloor;
        else if (ChunkType == ChunkTypes.Solid)
            _meshRenderer.material = Globals.ChunkMaterialSolid;
    }

    private void CheckAndAddSprite(ushort index)
    {
        PrimaryBlocks block = (PrimaryBlocks)ItemInfos.GetItemFromId(GetBlockId(index));
        if (block.GroundSprite == null)
            return;

        if (block.GroundSprite != null)
        {
            string layer = "SpriteObj";
            if (block.SpriteUnderPlayer)
                layer = "SpriteUnderPlayer";

            Vector2Int position = GetBlockPosition(index);
            GameObject spriteObj = Globals.SpritePool.GetSpriteObj(block.GroundSprite, new Vector3(position.x, position.y, 0), _chunkGameObject.transform, block.SpriteScale, block.SpriteOffset, layer);
            _sprites.Add(index, spriteObj);
        }
    }

    private void GenerateBlocks()
    {
        byte light = 1;
        for (ushort i = 0; i < _blocs.Length; i++)
        {
            Vector2Int worldPosition = new(Position.x * Globals.ChunkSize + i % Globals.ChunkSize, Position.y * Globals.ChunkSize + i / Globals.ChunkSize);
            if (ChunkType == ChunkTypes.Floor)
                _blocs[i] = new Block(Noise.GetFloorBlockAtWorldPosition(worldPosition, WorldId), light);
            else if (ChunkType == ChunkTypes.Solid)
            {
                _blocs[i] = new Block(Noise.GetSolidBlockAtWorldPosition(worldPosition, WorldId), light);
                CheckAndAddSprite(i);
            }
        }
    }

    private void GenerateVertices()
    {
        _vertices.Clear();
        _indices.Clear();
        _uvs.Clear();
        _uvts.Clear();
        _outlineNS.Clear();
        _outlineEW.Clear();

        bool[,] visited = new bool[Globals.ChunkSize, Globals.ChunkSize];

        for (int x = 0; x < Globals.ChunkSize; x++)
        {
            for (int y = 0; y < Globals.ChunkSize; y++)
            {
                if (visited[x, y]) continue;

                int blockPosition = x + y * Globals.ChunkSize;
                ushort blockId = _blocs[blockPosition].Id;
                ushort textureId = ((PrimaryBlocks)ItemInfos.GetItemFromId(blockId)).TextureId;
                byte lightValue = (byte)Mathf.Min(_blocs[blockPosition].Light, 255);

                if (blockId == ItemIds.Air || textureId == ushort.MaxValue)
                {
                    visited[x, y] = true;
                    continue;
                }

                int width = 1, height = 1;

                while (x + width < Globals.ChunkSize &&
                        _blocs[x + width + y * Globals.ChunkSize].Id == blockId &&
                        _blocs[x + width + y * Globals.ChunkSize].Light == lightValue &&
                        _blocs[x + width + y * Globals.ChunkSize].Outlines == _blocs[blockPosition].Outlines &&
                        !visited[x + width, y])
                {
                    width++;
                }

                bool validHeight = true;
                while (validHeight && y + height < Globals.ChunkSize)
                {
                    for (int i = 0; i < width; i++)
                    {
                        if (_blocs[x + i + (y + height) * Globals.ChunkSize].Id != blockId ||
                            _blocs[x + i + (y + height) * Globals.ChunkSize].Light != lightValue ||
                            _blocs[x + i + (y + height) * Globals.ChunkSize].Outlines != _blocs[blockPosition].Outlines ||
                            visited[x + i, y + height])
                        {
                            validHeight = false;
                            break;
                        }
                    }

                    if (validHeight) height++;
                }

                for (int i = x; i < x + width; i++)
                {
                    for (int j = y; j < y + height; j++)
                    {
                        visited[i, j] = true;
                    }
                }

                _vertices.AddRange(new Vector3[]
                {
                    new Vector3(x, y, 0),
                    new Vector3(x + width, y, 0),
                    new Vector3(x + width, y + height, 0),
                    new Vector3(x, y + height, 0)
                });

                _indices.AddRange(new int[]
                {
                    _vertices.Count - 4,
                    _vertices.Count - 2,
                    _vertices.Count - 3,
                    _vertices.Count - 4,
                    _vertices.Count - 1,
                    _vertices.Count - 2
                });

                _uvs.AddRange(new Vector2[]
                {
                    new Vector2(0f, 0f),
                    new Vector2(width, 0f),
                    new Vector2(width, height),
                    new Vector2(0f, height)
                });

                _uvts.AddRange(new Vector2[]
                {
                    new Vector2(textureId, lightValue),
                    new Vector2(textureId, lightValue),
                    new Vector2(textureId, lightValue),
                    new Vector2(textureId, lightValue)
                });

                byte currentOutlines = _blocs[blockPosition].Outlines;
                Vector2 northSouthOutline = new((currentOutlines & 0x8) >> 3, (currentOutlines & 0x4) >> 2);
                Vector2 eastWestOutline = new((currentOutlines & 0x2) >> 1, currentOutlines & 0x1);

                _outlineNS.AddRange(new Vector2[]
                {
                    northSouthOutline,
                    northSouthOutline,
                    northSouthOutline,
                    northSouthOutline
                });

                _outlineEW.AddRange(new Vector2[]
                {
                    eastWestOutline,
                    eastWestOutline,
                    eastWestOutline,
                    eastWestOutline
                });

            }
        }
    }

    private void GenerateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _indices.ToArray();
        _mesh.uv = _uvs.ToArray();
        _mesh.uv2 = _uvts.ToArray();
        _mesh.uv3 = _outlineNS.ToArray();
        _mesh.uv4 = _outlineEW.ToArray();
        _mesh.RecalculateNormals();
    }
}
