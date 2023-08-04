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
    private readonly List<Vector2> _uvts = new();
    private readonly List<MaterialPropertyBlock> _blocksProperties = new();
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private GameObject _chunkGameObject;
    private Mesh _mesh;

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
        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;
        _meshRenderer = _chunkGameObject.AddComponent<MeshRenderer>();
        _meshRenderer.material = Resources.Load<Material>("Materials/BlockMaterial");
    }

    private void GenerateBlocks()
    {
        for (int i = 0; i < Blocs.Length; i++)
        {
            if (Random.Range(0, 100) < 50)
                Blocs[i] = new Block(BlockIds.Grass);
            else
                Blocs[i] = new Block(BlockIds.Stone);
        }
    }

    private void GenerateVertices()
    {
        _vertices.Clear();
        _indices.Clear();
        _uvs.Clear();
        _uvts.Clear();
        _blocksProperties.Clear();

        bool[,] visited = new bool[Globals.ChunkSize, Globals.ChunkSize];

        for (int x = 0; x < Globals.ChunkSize; x++)
        {
            for (int y = 0; y < Globals.ChunkSize; y++)
            {
                if (visited[x, y]) continue;

                int blockPosition = x + y * Globals.ChunkSize;
                ushort blockId = Blocs[blockPosition].Id;
                ushort textureId = AllBlocksStats.GetBlockStatsFromId(blockId).TextureId;

                if (blockId == BlockIds.Air)
                {
                    visited[x, y] = true;
                    continue;
                }

                int width = 1, height = 1;

                while (x + width < Globals.ChunkSize &&
                       Blocs[x + width + y * Globals.ChunkSize].Id == blockId)
                {
                    width++;
                }

                bool validHeight = true;
                while (validHeight && y + height < Globals.ChunkSize)
                {
                    for (int i = 0; i < width; i++)
                    {
                        if (Blocs[x + i + (y + height) * Globals.ChunkSize].Id != blockId)
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
                    new Vector2(textureId, 0f),
                    new Vector2(textureId, 0f),
                    new Vector2(textureId, 0f),
                    new Vector2(textureId, 0f)
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
