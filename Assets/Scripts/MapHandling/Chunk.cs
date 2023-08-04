using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Block
{
    public ushort Id;

    public Block(ushort id)
    {
        Id = id;
    }
}

public enum ChunkTypes
{
    Floor,
    Solid
}

public class Chunk
{
    public Block[] Blocs = new Block[Globals.ChunkSize * Globals.ChunkSize];
    public Vector2Int Position;
    public WorldsIds WorldId;
    public ChunkTypes ChunkType;

    private readonly List<Vector3> _vertices = new();
    private readonly List<int> _indices = new();
    private readonly List<Vector2> _uvs = new();
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private GameObject _chunkGameObject;

    public Chunk(Vector2Int position, WorldsIds worldId, ChunkTypes chunkType)
    {
        Position = position;
        WorldId = worldId;
        ChunkType = chunkType;
        InitGameObject();
        GenerateBlocks();
        GenerateVertices();
        GenerateMesh();
    }

    public void ReloadChunk()
    {
        GenerateVertices();
        GenerateMesh();
    }

    public void RemoveBlock(Vector2Int position)
    {
        Blocs[position.x + position.y * Globals.ChunkSize] = new Block(BlockIds.Air);
        ReloadChunk();
    }

    public void AddBlock(Vector2Int position, ushort blockId)
    {
        Blocs[position.x + position.y * Globals.ChunkSize] = new Block(blockId);
        ReloadChunk();
    }

    private Transform GetParent()
    {
        GameObject world = Worlds.GetWorldFromId(WorldId);
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
        _meshRenderer = _chunkGameObject.AddComponent<MeshRenderer>();
        _meshRenderer.material = Resources.Load<Material>("Materials/BlockMaterial");
        Debug.Log(_meshRenderer.material);
    }

    private void GenerateBlocks()
    {
        for (int i = 0; i < Blocs.Length; i++)
        {
            Blocs[i] = new Block(BlockIds.Grass);
        }
    }

    private void GenerateVertices()
    {
        _vertices.Clear();
        _indices.Clear();
        _uvs.Clear();

        for (int x = 0; x < Globals.ChunkSize; x++)
        {
            for (int y = 0; y < Globals.ChunkSize; y++)
            {
                int blockPosition = x + y * Globals.ChunkSize;
                ushort blockId = Blocs[blockPosition].Id;
                if (blockId == BlockIds.Air)
                {
                    continue;
                }

                _vertices.AddRange(new Vector3[]
                {
                    new Vector3(x, y, 0),
                    new Vector3(x + 1, y, 0),
                    new Vector3(x + 1, y + 1, 0),
                    new Vector3(x, y + 1, 0)
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

                AddUVs(AllBlocksStats.GetBlockStatsFromId(blockId).TextureId);
            }
        }
    }

    private void AddUVs(int textureID)
    {
        float y = textureID / Globals.TextureAtlasSizeInBlocks;
        float x = textureID - (y * Globals.TextureAtlasSizeInBlocks);
        x *= Globals.NormalizedBlockTextureSize;
        y *= Globals.NormalizedBlockTextureSize;
        y = 1f - y - Globals.NormalizedBlockTextureSize;

        _uvs.AddRange(new Vector2[]
        {
            new Vector2(x, y),
            new Vector2(x + Globals.NormalizedBlockTextureSize, y),
            new Vector2(x + Globals.NormalizedBlockTextureSize, y + Globals.NormalizedBlockTextureSize),
            new Vector2(x, y + Globals.NormalizedBlockTextureSize)
        });
    }

    private void GenerateMesh()
    {
        Mesh mesh = new()
        {
            vertices = _vertices.ToArray(),
            triangles = _indices.ToArray(),
            uv = _uvs.ToArray()
        };
        mesh.RecalculateNormals();

        _meshFilter.mesh = mesh;
    }
}
