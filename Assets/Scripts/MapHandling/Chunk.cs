using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public struct Block
{
    public ushort Id;
    public byte Light;

    public Block(ushort id, byte light)
    {
        Id = id;
        Light = light;
    }
}

public enum ChunkTypes
{
    Floor,
    Solid
}

public struct ChunkModification
{
    public Vector2Int BlockPosition;
    public ushort Id;
    public short Light;

    public ChunkModification(Vector2Int blockPosition, ushort id, short light)
    {
        BlockPosition = blockPosition;
        Id = id;
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

    public void AddModification(Vector2Int blockPosition, ushort id, short light)
    {
        _modifications.Push(new ChunkModification(blockPosition, id, light));
    }

    private void ApplyModifications()
    {
        while (_modifications.Count > 0)
        {
            ChunkModification modification = _modifications.Pop();
            Block block = _blocs[modification.BlockPosition.x + modification.BlockPosition.y * Globals.ChunkSize];
            _blocs[modification.BlockPosition.x + modification.BlockPosition.y * Globals.ChunkSize] = new Block(modification.Id, (byte)(block.Light + modification.Light));
        }
    }

    public void ReloadChunk()
    {
        if (!_isLoaded)
        {
            InitGameObject();
            GenerateBlocks();
            _isLoaded = true;
        }

        ApplyModifications();
        GenerateVertices();
        GenerateMesh();
    }

    public bool IsLoaded()
    {
        return _isLoaded;
    }

    public ushort GetBlockId(Vector2Int position)
    {
        return _blocs[position.x + position.y * Globals.ChunkSize].Id;
    }

    private void CheckAndRemoveSprite(int index)
    {
        if (_sprites.ContainsKey(index))
        {
            Globals.SpritePool.ReturnSprite(_sprites[index]);
            _sprites.Remove(index);
        }
    }

    public void SetBlock(Vector2Int position, ushort blockId)
    {
        int index = position.x + position.y * Globals.ChunkSize;
        CheckAndRemoveSprite(index);
        _blocs[position.x + position.y * Globals.ChunkSize] = new Block(blockId, _blocs[position.x + position.y * Globals.ChunkSize].Light);
        CheckAndAddSprite(index, position);
        ReloadChunk();
    }

    private Transform GetParent()
    {
        GameObject world = WorldsHelper.GetWorldFromId(WorldId);
        if (ChunkType == ChunkTypes.Floor)
            return world.transform.Find("floor");
        else if (ChunkType == ChunkTypes.Solid)
            return world.transform.Find("solid");
        throw new System.Exception("Chunk type does not exist or child not found");
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

    private void CheckAndAddSprite(int index, Vector2Int localPosition)
    {
        PrimaryBlocks block = (PrimaryBlocks)ItemInfos.GetItemFromId(GetBlockId(localPosition));
        if (block.GetType() != typeof(SpritePrimaryBlocks))
            return;

        SpritePrimaryBlocks spriteBlock = (SpritePrimaryBlocks)block;
        if (spriteBlock.GroundSprite != null)
        {
            string layer = "SpriteObj";
            if (spriteBlock.SpriteUnderPlayer)
                layer = "SpriteUnderPlayer";

            GameObject spriteObj = Globals.SpritePool.GetSpriteObj(spriteBlock.GroundSprite, new Vector3(localPosition.x, localPosition.y, 0), _chunkGameObject.transform, spriteBlock.SpriteScale, spriteBlock.SpriteOffset, layer);
            _sprites.Add(index, spriteObj);
        }
    }

    private void GenerateBlocks()
    {
        byte light = 1;
        for (int i = 0; i < _blocs.Length; i++)
        {
            Vector2Int worldPosition = new(Position.x * Globals.ChunkSize + i % Globals.ChunkSize, Position.y * Globals.ChunkSize + i / Globals.ChunkSize);
            if (ChunkType == ChunkTypes.Floor)
                _blocs[i] = new Block(Noise.GetFloorBlockAtWorldPosition(worldPosition, WorldId), light);
            else if (ChunkType == ChunkTypes.Solid)
            {
                _blocs[i] = new Block(Noise.GetSolidBlockAtWorldPosition(worldPosition, WorldId), light);
                CheckAndAddSprite(i, new Vector2Int(i % Globals.ChunkSize, i / Globals.ChunkSize));
            }
        }
    }

    private void GenerateVertices()
    {
        _vertices.Clear();
        _indices.Clear();
        _uvs.Clear();
        _uvts.Clear();

        bool[,] visited = new bool[Globals.ChunkSize, Globals.ChunkSize];

        for (int x = 0; x < Globals.ChunkSize; x++)
        {
            for (int y = 0; y < Globals.ChunkSize; y++)
            {
                if (visited[x, y]) continue;

                int blockPosition = x + y * Globals.ChunkSize;
                ushort blockId = _blocs[blockPosition].Id;
                ushort textureId = ((PrimaryBlocks)ItemInfos.GetItemFromId(blockId)).TextureId;
                byte lightValue = (byte)Mathf.Min(_blocs[blockPosition].Light, 32);

                if (blockId == ItemIds.Air || textureId == ushort.MaxValue)
                {
                    visited[x, y] = true;
                    continue;
                }

                int width = 1, height = 1;

                while (x + width < Globals.ChunkSize &&
                        _blocs[x + width + y * Globals.ChunkSize].Id == blockId &&
                        _blocs[x + width + y * Globals.ChunkSize].Light == lightValue &&
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
        _mesh.RecalculateNormals();
    }
}
